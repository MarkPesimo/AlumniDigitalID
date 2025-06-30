$(function () {
    $(document).ready(function () {

        ShowLoading('HIDE');
    });

 
    //$("#manage-shortcut-btn").click(function (e) {
    //    e.preventDefault();

    //    $('#filter_payslip_modal').modal('show');
    //});
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

    $("#cancel-update-button").click(function (e) {
        window.location.href = "/Alumni/Index";
    });


    $("#alumni-profile-image").click(function (e) {
        $("#change_image_modal").modal('show');
        var _attach = document.getElementById('#change_profile_Attachment');
        _attach.Value = "";
    });


    $(".profile-banner").click(function (e) {
        $("#change_image_modal").modal('show');
        var _attach = document.getElementById('#change_profile_Attachment');
        _attach.Value = "";
    });


    $('#change_image_modal').on('click', '#save-picture-button', function (e) {
  
        $file = $("#change_profile_Attachment");
        var $filepath = $.trim($file.val());
        if ($filepath == "") {
            ShowWarningMessage('Please select a file, Attachment is required.')
            return;
        }

        var formData = new FormData();
        var _Attachement = $("#change_profile_Attachment")[0].files[0];
        formData.append('Attachment', _Attachement);

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
        $("#update-candidate-social-modal").modal('hide');

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
        return;
    }

    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') { x.style.visibility = ''; }
        else { x.style.visibility = 'hidden'; }
    }
});