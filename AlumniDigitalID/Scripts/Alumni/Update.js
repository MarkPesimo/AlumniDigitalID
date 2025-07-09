$(function () {
    $(document).ready(function () {
        ShowLoading('HIDE');
    });

    $("#change_profile_Attachment").change(function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $("#preview-profile-image").attr("src", e.target.result);
            };
            reader.readAsDataURL(file);
        }
    });


    $("#submit-update-button").click(function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/Update',
            type: "POST",
            data: $('#alumni-profile-update').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    ShowLoading('HIDE');
                    setTimeout(ShowSuccessMessage('Alumni information successfully updated.'), 5000);
                    window.location.href = "/Alumni/Index";
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $("#cancel-update-button").click(function () {
        window.location.href = "/Alumni/Index";
    });

    // Only bind to image
    $("#alumni-profile-image").click(function (e) {
        e.stopPropagation();
        $("#change_image_modal").modal('show');
        var _attach = document.getElementById('change_profile_Attachment');
        _attach.value = "";
    });

    // Removed this to avoid modal on banner click
    // $(".profile-banner").click(function (e) {
    //     $("#change_image_modal").modal('show');
    // });

    $('#change_image_modal').on('click', '#save-picture-button', function () {
        let $file = $("#change_profile_Attachment");
        let filepath = $.trim($file.val());
        if (filepath === "") {
            ShowWarningMessage('Please select a file, Attachment is required.');
            return;
        }

        let formData = new FormData();
        let _attachment = $file[0].files[0];
        formData.append('Attachment', _attachment);

        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/_Attachment',
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#change_image_modal").modal('hide');
                    ShowLoading('HIDE');
                    ShowSuccessMessage('Alumni profile picture successfully updated.');
                    ReloadImage(result._guid);
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    function ReloadImage(guid) {
        document.getElementById('alumni-profile-image').src = "\\AlumniImages\\" + guid + ".JPEG?random=" + new Date().getTime();
    }

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
    }

    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        x.style.visibility = (show === 'SHOW') ? '' : 'hidden';
    }
});
