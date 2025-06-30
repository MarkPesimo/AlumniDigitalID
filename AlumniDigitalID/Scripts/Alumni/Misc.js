$(function () {
    $(document).ready(function () {
        ShowLoading('HIDE');
    });

    $("#submit-misc-update-button").click(function (e) {
        var _id = document.getElementById("Id").value;
        var _description = document.getElementById("Description").value;
        var _type = document.getElementById("Type").value;

        ShowLoading('SHOW');

        $.ajax({
            url: '/Alumni/Misc',
            type: "POST",
            data: {
                '_id': _id,
                '_type': _type    ,
                '_description': _description
            },
            dataType: "json",
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    ShowLoading('HIDE');
                    ShowSuccessMessage(result.Type + ' successfully updated.');
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $("#cancel-misc-update-button").click(function (e) {
        window.location.href = "/Alumni/Index";
    });

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