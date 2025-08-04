$(function () {
    $(document).ready(function () {
        // Initialize DataTable with search functionality
        var table = $('#members-table').DataTable({
            dom: '<"top"l>rt<"bottom"ip>',
            processing: true,
            serverSide: false,
            searching: true,
            lengthChange: false,
            //columns: [
            //    {
            //        data: null,
            //        render: function (data, type, row) {
            //            // Member information display with image
            //            return `
            //            <div class="row">
            //                <div class="col-auto">
            //                    <img src="/AlumniImages/${row.Guid}.JPEG"
            //                         class="alumni-members-image"
            //                         alt="${row.MembersName}"
            //                         onerror="this.src='/AlumniImages/0.JPEG'">
            //                </div>
            //                <div class="col">
            //                    <strong class="text-primary">${row.MembersName}</strong><br>
            //                    ${row.Course}<br>
            //                    <small>${row.MemberType}</small><br>
            //                    <small>Batch ${row.Batch}</small>
            //                </div>
            //            </div>
            //        `;
            //        }
            //    },
            //    {
            //        data: null,
            //        orderable: false,
            //        render: function (data, type, row) {
            //            // Determine status based on which button is enabled
            //            const isActive = row.Activate_prop === 'disabled';
            //            const statusText = isActive ? 'Active' : 'Inactive';
            //            const statusClass = isActive ? 'success' : 'danger';

            //            return `
            //            <div class="btn-group btn-group-sm" role="group">
            //                <button class="btn btn-sm btn-secondary edit-member-btn" ${row.Edit_prop} guid="${row.Guid}">
            //                    <i class="bi bi-pencil-square"></i>
            //                </button>
            //                <span class="badge bg-${statusClass} align-self-center">
            //                    ${statusText}
            //                </span>
            //            </div>
            //        `;
            //        }
            //    }
            //],
            language: {
                search: "",
                searchPlaceholder: "Search members..."
            }
        });

        // Hide default length dropdown
        $('.dt-length').addClass('d-none');

        // Custom search functionality
        $('#member-search').on('keyup', function () {
            table.search(this.value).draw();
        });
    });

    //===============================================BEGIN EDIT==================================================================================
    $('#members-table').on('click', '.edit-member-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/Edit',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#edit_member_modal').find(".modal-body").innerHTML = '';
                $('#edit_member_modal').find(".modal-body").html(response);
                $("#edit_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#edit_member_modal').on('click', '#edit-member-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/Edit',
            type: "POST",
            data: $('#edit-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    ShowSuccessMessage('Member information successfully updated.')
                    $("#edit_member_modal").modal('hide');
                    ShowLoading("HIDE");
                }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END EDIT==================================================================================

    //===============================================BEGIN ACTIVATE==================================================================================
    $('#members-table').on('click', '.activate-member-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/_Activate',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#activate_member_modal').find(".modal-body").innerHTML = '';
                $('#activate_member_modal').find(".modal-body").html(response);
                $("#activate_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#activate_member_modal').on('click', '#activate-member-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/ActivateDeactivate',
            type: "POST",
            data: $('#activate-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else { window.location.href = "/Alumni/Members"; }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END ACTIVATE==================================================================================


    //===============================================BEGIN DEACTIVATE==================================================================================
    $('#members-table').on('click', '.deactivate-member-btn', function () {
        var _guid = $(this).attr("guid");

        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/_Deactivate',
            data: { '_guid': _guid },
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#deactivate_member_modal').find(".modal-body").innerHTML = '';
                $('#deactivate_member_modal').find(".modal-body").html(response);
                $("#deactivate_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#deactivate_member_modal').on('click', '#deactivate-member-button', function (e) {
        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/ActivateDeactivate',
            type: "POST",
            data: $('#deactivate-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else { window.location.href = "/Alumni/Members"; }
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END DEACTIVATE==================================================================================

    //===============================================BEGIN ADD==================================================================================
    $("#add-member-btn").click(function (e) {
        ShowLoading('SHOW');
        $.ajax({
            type: "GET",
            url: '/Alumni/_Add',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {

                $('#add_member_modal').find(".modal-body").innerHTML = '';
                $('#add_member_modal').find(".modal-body").html(response);
                $("#add_member_modal").modal('show');
                ShowLoading('HIDE');
            },
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });

    $('#add_member_modal').on('click', '#add-members-button', function (e) {
        //$file = $("#add_perks_Attachment");
        //var $filepath = $.trim($file.val());
        //if ($filepath == "") {
        //    ShowWarningMessage('Please select a file, Attachment is required.')
        //    return;
        //}
        let $file = $("#Alumni_Attachment");
        let filepath = $.trim($file.val());
        if (filepath === "") {
            ShowWarningMessage('Please select a image/picture, Attachment is required.');
            return;
        }

        ShowLoading('SHOW');
        $.ajax({
            url: '/Alumni/_Add',
            type: "POST",
            data: $('#add-member-form').serialize(),
            dataType: 'json',
            success: function (result) {
                if (result.Result == "ERROR") { ValidationError(result); }
                else {
                    $("#add_member_modal").modal('hide');

                    let formData = new FormData();
                    let _attachment = $file[0].files[0];
                    formData.append('Attachment', _attachment);
                    formData.append('_addguid', result.Guid)

                    ShowLoading('SHOW');
                    $.ajax({
                        url: '/Alumni/_AddAttachment',
                        type: "POST",
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (result) {
                            if (result.Result == "ERROR") { ValidationError(result); }
                            else {
                                ShowSuccessMessage('Alumni member successfully created.');
                            }
                        },
                        failure: function (response) { LogError(response); },
                        error: function (response) { LogError(response); }
                    });
                }
            },            
            failure: function (response) { LogError(response); },
            error: function (response) { LogError(response); }
        });
    });
    //===============================================END ADD==================================================================================


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