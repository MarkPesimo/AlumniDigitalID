$(function () {
    $(document).ready(function () {
        ShowLoading('HIDE');
    });


    $("#login-btn").click(function (e) {
        e.preventDefault();

        var btn = document.getElementById("login-btn");
        btn.disabled = true;

        var _id = e.target.id;
        ShowLoading('SHOW');
        $.ajax({
            url: '/Account/Login',
            type: "POST",
            data: $('#login-Form').serialize(),
            dataType: 'json',
            success: function (result) {
                ShowLoading('HIDE');
                if (result.Result == "ERROR") {
                    btn.disabled = false;
                    ValidationError(result);
                }
                else {
                    if (navigator.geolocation) {
                        navigator.geolocation.getCurrentPosition(
                            function (position) {
                                $.ajax({
                                    url: '/Account/LogLogin',
                                    type: "POST",
                                    data:
                                    {
                                        '_userid': result.UserId,
                                        '_longitude': position.coords.longitude,
                                        '_latitude': position.coords.latitude
                                    },
                                    dataType: 'json',
                                    success: function (x) {
                                        window.location = result.URL;
                                    }
                                });
                            },
                            function () {
                                $.ajax({
                                    url: '/Account/LogLogin',
                                    type: "POST",
                                    data:
                                    {
                                        '_userid': result.UserId,
                                        '_longitude': 0,
                                        '_latitude': 0
                                    },
                                    dataType: 'json',
                                    success: function (x) {
                                        window.location = result.URL;
                                    }
                                });
                            });
                    }
                    else { window.location = result.URL; }                    
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });


    ////=========================================NEW CHANGE PASSWORD========================================

    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') { x.style.visibility = ''; }
        else { x.style.visibility = 'hidden'; }
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
});