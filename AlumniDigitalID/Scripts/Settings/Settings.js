$(function () {
    $(document).ready(function () {
        ShowLoading('HIDE');
    });

    // Open Change PIN modal via AJAX
    $("#change_pin_btn").click(function (e) {
        e.preventDefault();

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Settings/ChangePIN',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                ShowLoading('HIDE');
                $('#change_pin_modal').find(".modal-body").html(response);
                $("#change_pin_modal").modal('show');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    // Save PIN handler
    $('#change_pin_modal').on('click', '#save-pin-button', function () {
        ShowLoading('SHOW');

        const pin1 = $('input[name="pin1"]').val();
        const pin2 = $('input[name="pin2"]').val();
        const pin3 = $('input[name="pin3"]').val();
        const pin4 = $('input[name="pin4"]').val();
        const fullPin = `${pin1}${pin2}${pin3}${pin4}`;

        if (!/^\d{4}$/.test(fullPin)) {
            ShowLoading('HIDE');
            alert("Please enter a valid 4-digit numeric PIN.");
            return;
        }

        $('#full-pin').val(fullPin); // Assign to hidden input before submit

        $.ajax({
            url: '/Settings/Update',
            type: "POST",
            data: $('#change-pin-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result === "ERROR") {
                    ValidationError(result);
                } else {
                    $("#change_pin_modal").modal('hide');
                    ShowLoading('HIDE');
                    ShowSuccessMessage('PIN number successfully updated.');
                }
            },
            error: function (response) {
                LogError(response);
            }
        });
    });

    // Auto-tab and restrict to single digit input
    $(document).on('input', '.pin-box', function () {
        const val = $(this).val();
        if (!/^\d$/.test(val)) {
            $(this).val('');
            return;
        }
        $(this).next('.pin-box').focus();
    });

    // Allow backspace to move to previous field
    $(document).on('keydown', '.pin-box', function (e) {
        if (e.key === "Backspace" && !$(this).val()) {
            $(this).prev('.pin-box').focus();
        }
    });

    // Block paste and block non-digit keys
    $(document).on('paste keypress', '.pin-box', function (e) {
        if (e.type === 'keypress' && !/\d/.test(e.key)) {
            e.preventDefault();
        } else if (e.type === 'paste') {
            e.preventDefault();
        }
    });

    // Enforce digits only, and auto tab to next
    $(document).on('input', '.pin-box', function () {
        const val = $(this).val();
        // Remove non-digit characters
        $(this).val(val.replace(/\D/g, ''));

        // Move to next if valid digit
        if (/^\d$/.test($(this).val())) {
            $(this).next('.pin-box').focus();
        }
    });

    // Move to previous input on backspace
    $(document).on('keydown', '.pin-box', function (e) {
        if (e.key === "Backspace" && !$(this).val()) {
            $(this).prev('.pin-box').focus();
        }

        // Prevent letters and special keys
        const allowedKeys = ['Backspace', 'Tab', 'ArrowLeft', 'ArrowRight', 'Delete'];
        if (
            !/^\d$/.test(e.key) &&
            !allowedKeys.includes(e.key)
        ) {
            e.preventDefault();
        }
    });

    // Prevent paste
    $(document).on('paste', '.pin-box', function (e) {
        e.preventDefault();
    });


    // ========================== MISC UTILS ==========================

    function ShowSuccessMessage(_msg) {
        ShowLoading('HIDE');
        document.getElementById("toasterSuccess-body").innerHTML = _msg;
        const toaster = document.getElementById("toasterSuccess");
        const toasterFunction = bootstrap.Toast.getOrCreateInstance(toaster);
        toasterFunction.show();
    }

    function LogError(response) {
        ShowLoading('HIDE');
        console.error(response.responseText);
    }

    function ValidationError(result) {
        if (result.ElementName != null) {
            var div_validation = document.querySelector('#div-validation');
            div_validation.style.display = "block";
            document.getElementsByName(result.ElementName)[0].focus();
            document.getElementById("error-message-label").innerHTML = "* " + result.Message;
        } else {
            alert(result.Message);
        }
        ShowLoading('HIDE');
    }

    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') {
            x.style.visibility = '';
        } else {
            x.style.visibility = 'hidden';
        }
    }
});
