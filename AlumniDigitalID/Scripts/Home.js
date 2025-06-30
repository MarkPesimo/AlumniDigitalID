$(function () {
    $(document).ready(function () {
        ShowLoading('HIDE');
    });


    function ShowLoading(show) {
        var x = document.getElementById("preloader");
        if (show === 'SHOW') { x.style.visibility = ''; }
        else { x.style.visibility = 'hidden'; }
    }
       
    $("#generate-qrcode-button").click(function (e) {
        ShowLoading('SHOW');
        $.ajax({
            type: "POST",
            url: ' https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=Example',
            data: {           
                "data": "https://www.qrcode-monkey.com",
                "config": {
                    "body": "circle",
                    "logo": "#facebook"
                },
                "size": 300,
                "download": false,
                "file": "svg"
            },
            dataType: "html",
            success: function (response) {
                alert(response);
                console.log(response);
                 
                ShowLoading('HIDE');
            },
            failure: function (response) { console.log(response); },
            error: function (response) { console.log(response); }
        });
    });
    //$("#manage-shortcut-btn").click(function (e) {
    //    e.preventDefault();

    //    $('#filter_payslip_modal').modal('show');
    //});

});