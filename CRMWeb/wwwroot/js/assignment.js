var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/taskassignment/GetAllTaskAssignment",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [           
            { "data": "account.name", "width": "20%" },
            { "data": "task.name", "width": "15%" },
            { "data": "employee.name", "width": "15%" },
            { "data": "department.name", "width": "8%" },
            {"data": "dueDate", "width": "10%", "type": 'datetime',"moment(data).format":"('MMMM Do YYYY')"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/taskassignment/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/taskassignment/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}