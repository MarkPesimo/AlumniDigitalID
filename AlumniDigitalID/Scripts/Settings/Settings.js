$(function () {
    $(document).ready(function () {
        ShowLoading('HIDE');
    });

    $("#change_username_btn").click(function (e) {
        e.preventDefault();

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Settings/ChangeUsername',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                ShowLoading('HIDE');
                $('#change_username_modal').find(".modal-body").innerHTML = '';
                $('#change_username_modal').find(".modal-body").html(response);
                $("#change_username_modal").modal('show');
               
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $("#change_password_btn").click(function (e) {
        e.preventDefault();

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Settings/ChangePassword',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                ShowLoading('HIDE');
                $('#change_password_modal').find(".modal-body").innerHTML = '';
                $('#change_password_modal').find(".modal-body").html(response);
                $("#change_password_modal").modal('show');

            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

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
                $('#change_pin_modal').find(".modal-body").innerHTML = '';
                $('#change_pin_modal').find(".modal-body").html(response);
                $("#change_pin_modal").modal('show');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });


    $('#change_username_modal').on('click', '#save-username-button', function () {

        ShowLoading('SHOW');
        $.ajax({
            url: '/Settings/Update',
            type: "POST",
            data: $('#change-username-form').serialize(),
            dataType: 'json',
            success: function (result) {
                //console.log(result);
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    //$("#change_username_modal").modal('hide');

                    //ShowLoading('HIDE');
                    //ShowSuccessMessage('Username successfully updated.');
                    window.location.href = "/Home/Success";
                }
            }
        });
    });

    $('#change_password_modal').on('click', '#save-password-button', function () {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Settings/Update',
            type: "POST",
            data: $('#change-password-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#change_password_modal").modal('hide');

                    ShowLoading('HIDE');
                    ShowSuccessMessage('Password successfully updated.');
                }
            }
        });
    });

    $('#change_pin_modal').on('click', '#save-pin-button', function () {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Settings/Update',
            type: "POST",
            data: $('#change-pin-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#change_pin_modal").modal('hide');

                    ShowLoading('HIDE');
                    ShowSuccessMessage('PIN number successfully updated.');
                }
            }
        });
    });

    //==================================BEGIN MISC==================================
    function ShowSuccessMessage(_msg) {
        ShowLoading('HIDE');
        document.getElementById("toasterSuccess-body").innerHTML = _msg;
        const toaster = document.getElementById("toasterSuccess");
        const toasterFunction = bootstrap.Toast.getOrCreateInstance(toaster);
        toasterFunction.show();
    }


    function LogError(response) {
        ShowLoading('HIDE');
        console.log(response.responseText);
    }

    function ValidationError(result) {
        if (result.ElementName != null) {
            var div_validation = document.querySelector('#div-validation');
            div_validation.style.display = "block";

            document.getElementsByName(result.ElementName)[0].focus();
            document.getElementById("error-message-label").innerHTML = "* " + result.Message;
        }
        else { window.alert(result.Message); }
        ShowLoading('HIDE');
        return;
    }

    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') { x.style.visibility = ''; }
        else { x.style.visibility = 'hidden'; }
    }

    //$("#manage-shortcut-btn").click(function (e) {
    //    e.preventDefault();

    //    $('#filter_payslip_modal').modal('show');
    //});

});