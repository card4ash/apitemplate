$(function () {
    var $loading = $('#overlay').hide();
    LoadData();
});

function LoadData() {
    $('#userTable').DataTable().destroy();
    $("#userTable").DataTable({
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
        "autoWidth": true,
        "ajax": {
            "url": "https://localhost:7156/api/users",
            "headers": {
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                'Access-Control-Allow-Origin':'*'
            },
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
       
        "columns": [
            {
                "render": function (data, type, row) {
                    return '<div style="width:110px;"> <a data-id="' + row.id +'"  href="/post/index?userId='+row.id+'"  class="btnDetails btn btn-icon btn-outline-primary btn-round btn-sm" data-rel="tooltip" title="View"><i class="ft-eye"></i></a></div>';
                }
                , "orderable": false
            },
            { "data": "name", "name": "name" },
            { "data": "username", "name": "username" },
            { "data": "email", "name": "email" },
            { "data": "phone", "name": "phone" }, 
            { "data": "website", "name": "website" },
            {
                "data": "company.name", "name": "company.name"
            }
        ]
    });
}




