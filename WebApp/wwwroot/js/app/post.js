$(function () {
    var $loading = $('#overlay').hide();
    var userId = $('#hdUserId').val();
    LoadData(userId);
    $.ajax({
        url: 'https://localhost:7156/api/users',
        dataType: 'JSON',
        headers: {
            'Authorization': 'Bearer '+ window.localStorage.getItem('token')
        },
        contentType: "application/json",
        type: "GET",
        data: { id: userId },
        success: function (response) {
            if (response && response.length) {
                var user = response[0];
                $('#name').text(user.name);
                $('#username').text(user.username);
                $('#email').text(user.email);
                $('#phone').text(user.phone);
                $('#website').text(user.website);
                let address = getObjectString(user.address, 0);
                $('#address').html(address);
                let company = getObjectString(user.company);
                $('#company').html(company);
            }
        },
        error: function (ex) {
            console.error(ex);
        }
    });
});

function prettifyCamelCase(str) {
    var output = "";
    var len = str.length;
    var char;

    for (var i = 0; i < len; i++) {
        char = str.charAt(i);

        if (i == 0) {
            output += char.toUpperCase();
        }
        else if (char !== char.toLowerCase() && char === char.toUpperCase()) {
            output += " " + char;
        }
        else if (char == "-" || char == "_") {
            output += " ";
        }
        else {
            output += char;
        }
    }

    return output;
}

function getObjectString(obj,counter) {
    var objstringfy = '';
    for (const [key, value] of Object.entries(obj)) {
        if (typeof value === 'object' && !Array.isArray(value) && value !== null) {
            let str = "&nbsp;";
            let multiStr = str.repeat(4 * counter);
            let pritykey = prettifyCamelCase(key);
            objstringfy += multiStr + pritykey + ' : ' + '<br />'
            counter++;
            objstringfy += getObjectString(value, counter)
        }
        else {
            let str = "&nbsp;";
            let multiStr = str.repeat(4 * counter);
            let pritykey = prettifyCamelCase(key);
            objstringfy += multiStr + pritykey + ' : ' + value + '<br />';
        }
    }
    return objstringfy;
}

function LoadData(userId) {
    $('#postTable').DataTable().destroy();
    $("#postTable").DataTable({
        "processing": true, // for show progress bar
        "language": {
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '
        },
        "serverSide": false, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        //"order": [[0, "asc"]],
        "pageLength": 10,
        "pagingType": "full_numbers",
        "scrollX": true,
        "autoWidth": false,
        "ajax": {
            "url": "https://localhost:7156/api/posts",
            "type": "GET",
            "headers": {
                'Authorization': 'Bearer ' + window.localStorage.getItem('token')
            },
            "data": { userId: userId },
            "dataType": "json",
            "dataSrc": ""
        },
       
        "columns": [
            {
                "render": function (data, type, row) {
                    return '<div style="width:110px;"> <a data-id="' + row.id + '"  href="#"  class="btnDetails btn btn-icon btn-outline-primary btn-round btn-sm" data-rel="tooltip" title="View"><i class="ft-eye"></i></a></div>';
                }
                , "orderable": false
            },
            {
                "data": "title",
                "name": "title",
                "render": function (data, type, full, meta) {
                    return "<div class='text-wrap'>" + data + "</div>";
                }
            },
            {
                "data": "body",
                "name": "body",
                "render": function (data, type, full, meta) {
                    return "<div class='text-wrap'>" + data + "</div>";
                }
            }
        ]
    });
}




