$(document).ready(function () {
    initDataTable('UserTemplate', 'userDatatable', 'usersTableContainer', 'userTableLoadingBar', getAllUsersUrl, function (settings) {
        $('.selectpicker').off('change');
        $('.selectpicker').on('change', function () {
            var role = $(this).find("option:selected").val();
            var userId = $(this).find("option:selected").attr("data-id");
            updateRole(userId, role);
        });
    });

    
});
function deleteUserButtonClick(id, userName) {
    swal({
        title: 'Are you sure you want to delete ' + userName + '?',
        type: 'error',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then(function () {
        $.ajax({
            url: '/users/' + id,
            type: 'Delete',
            success: function (result) {
                deleteEntityFromTable($('#userDatatable'), $('[data-user-id="' + id + '"]'));
                swal({
                    title: 'The user was deleted',
                    text: result.message,
                    type: 'success'
                }).catch(swal.noop);
            },
            error: function (result) {
                console.log('SESSION_ERROR: ' + result.responseText);
            }
        });
    }).catch(swal.noop);
}

function updateRole(userId, role) {
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: '/users/' + userId + '/role',
        type: 'Patch',
        data: JSON.stringify({ 'role': role }),
        dataType: "json",
        success: function (result) {
            swal({
                title: 'Role was updated to ' + role,
                text: result.message,
                type: 'success',
                confirmButtonClass: 'addId'
            }).catch(swal.noop);
        },
        error: function (result) {
            console.log('SESSION_ERROR: ' + result.responseText);
        }
    });
}

function changeBannedState(userId, button) {
    var isBanned = true;
    
    if ($(button).hasClass('btn-success')) {
        isBanned = false;
    }
    
    $.ajax({
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        url: '/users/' + userId + '/IsBanned',
        type: 'Patch',
        data: JSON.stringify({ 'isBanned': isBanned }),
        dataType: "json",
        success: function (result) {
            var span = $(button).find('span').first();
            if (isBanned) {
                $(button).removeClass('btn-warning');
                $(button).addClass('btn-success');
                $(span).removeClass('fa-lock');
                $(span).addClass('fa-unlock');
            } else {
                $(button).removeClass('btn-success');
                $(button).addClass('btn-warning');
                $(span).removeClass('fa-unlock');
                $(span).addClass('fa-lock');
            }
            swal({
                title: isBanned ? 'User has been banned' : 'User has been unbanned',
                type: 'success'
            }).catch(swal.noop);
        },
        error: function (result) {
            console.log('SESSION_ERROR: ' + result.responseText);
        }
    });
}