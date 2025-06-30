$(function () {
    $(document).ready(function () {
 
        ShowLoading('HIDE');
    });


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