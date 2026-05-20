$(function () {



    $("#submit-renewal-btn").click(function (e) {
        e.preventDefault();

        var btn = document.getElementById("submit-renewal-btn");
        btn.disabled = true;

        ShowLoading('SHOW');
        $.ajax({
            url: '/Account/MembershipRenewal',
            type: "POST",
            data: $('#membershiprenewal-Form').serialize(),
            dataType: 'json',
            dataType: "html",
            success: function (response) {
                ShowLoading('HIDE');
                if (result.Result == "ERROR") {
                    btn.disabled = false;
                    ValidationError(result);
                }
                else if (result.Result == "SUCCES") {
                    window.location = result.URL;
                }                
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') { x.style.visibility = ''; }
        else { x.style.visibility = 'hidden'; }
    }

    function ShowSuccessMessage(_msg) {
        ShowLoading('HIDE');
        document.getElementById("toasterSuccess-body").innerHTML = _msg;
        const toaster = document.getElementById("toasterSuccess");
        const toasterFunction = bootstrap.Toast.getOrCreateInstance(toaster);
        toasterFunction.show();
    }

    function ShowWarningMessage(_msg) {
        ShowLoading('HIDE');
        document.getElementById("toasterWarning-body").innerHTML = _msg;
        const toaster = document.getElementById("toasterWarning");
        const toasterFunction = bootstrap.Toast.getOrCreateInstance(toaster);
        toasterFunction.show();
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
    function LogError(response) {
        ShowLoading('HIDE');
        console.log(response.responseText);
    }
});