function deleteKweetButtonClick(id) {
    
    $.ajax({
        url: '/kweets/' + id,
        type: 'Delete',
        success: function (result) {
            $('[data-kweet-id="' + id + '"]').remove();
            swal({
                title: 'The kweet was deleted',
                type: 'success'
            }).catch(swal.noop);
        },
        error: function (result) {
            console.log('SESSION_ERROR: ' + result.responseText);
        }
    });
}

function loadMoreKweets(userId) {
    var kweetCount = $('[data-kweet-id]').length;
    $.ajax({
        type: 'GET',
        url: '/kweets/newest/' + userId + '?From=' + kweetCount + '&To=' + (kweetCount + 10),
        success: function (array) {
            var source = $('#kweetTemplate').html();
            var template = Handlebars.compile(source);

            var i = 0;
            while (i < array.length) {
                var row = template(array[i]);
                $('#kweetsBody').append(row);
                i++;
            }
            
        }
    });
}