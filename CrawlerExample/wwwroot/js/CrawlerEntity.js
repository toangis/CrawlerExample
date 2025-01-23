$(document).ready(function () {
        $.ajaxSetup({
            async: false
        });

        $("body").children().each(function () {
            document.body.innerHTML = document.body.innerHTML.replace(/\u2028/g, ' ');
        });

        var tbsearch = $("#example").DataTable({
            "language": {
                "url": '/datatables.vietnam.json'
            },
            "bFilter": true,
            "scrollY": '300px', //scroll bar
            "scrollCollapse": true,
            "scrollX": true,
            "filter": true,
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Home/LoadData",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
                [{ "searchable": true, "targets": 1 }],
            "aLengthMenu": [[5, 10, 15], [5, 10, 15]], //Dropdown chọn số dòng hiển thị trong bảng. VD: 5 dòng, 10 dòng, 15 dòng
            "iDisplayLength": 5, //Ban đàu hiển thị 5 dòng
            "columns": [
                { "data": "Title", "title": "Title", "name": "Title", "autoWidth": true },
                { "data": "Summary", "title": "Summary", "name": "Summary", "autoWidth": true },
                { "data": "Votes", "title": "Votes", "name": "Votes", "autoWidth": true },
                { "data": "Views", "title": "Views", "name": "Views", "autoWidth": true },
                { "data": "Answers", "title": "Answers", "name": "Answers", "autoWidth": true },
                {
                    "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Home/Edit/' + full.id + '">Sửa</a>'; }
                },
                {
                    "render": function (data, type, row) {
                        return "<a href='#' class='btn btn-danger' " + ">Xóa</a>";
                    }
                },
            ]
        });

        $("#saveCrawler").click(function () {
            $.ajax({
                url: "/Home/LuuQuesion",
                type: 'GET',
                success: function (result) {
                    if (result) {
                        alert('Lưu dữ liệu vào dbo.Question trong database AppDbContext thành công');
                    }
                },
                failure: function (response) {
                    alert("Đã có lỗi: " + response.responseText);
                },
                error: function (err) {
                    alert("Đã có lỗi xảy ra " + err.statusText);
                }
            });
        });

        $("#test").click(function () {
            $.ajax({
                url: "/Home/GetQuestionEntity",
                type: 'GET',
                datatype: 'json',
                success: function (data) {
                    var data = JSON.parse(data);
                    console.log(data.length);
                    alert('Tiêu đề của Question: ' + data[0].Title);
                },
                failure: function (response) {
                    alert("Đã có lỗi với dữ liệu crawler: " + response.responseText);
                },
                error: function (err) {
                    alert("Đã có lỗi xảy ra " + err.statusText);
                }
            })
        });
    //Lấy dữ liệu sử dụng Entity Framework
    var tbEntity = $("#entityFramework").DataTable({
            "language": {
                "url": '/datatables.vietnam.json'
            },
            "bFilter": true,
            "scrollY": '500px', 
            "scrollCollapse": true,
            "scrollX": true,
            "filter": true,
            "processing": true, 
            "serverSide": true, 
            "orderMulti": false,
            "deferRender": true,
            "ajax": {
                "url": "/Home/LoadDataFromEntityFramework",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
                [{ "searchable": true, "targets": 1 }, { width: 200, targets: 1 }
                    ],
            "aLengthMenu": [[5, 10, 15], [5, 10, 15]], 
            "iDisplayLength": 10,
            "columns": [
                { "data": "Title", "title": "Title", "name": "Title", "autoWidth": true },
                { "data": "Summary", "title": "Summary", "name": "Summary", "autoWidth": true },
                { "data": "Votes", "title": "Votes", "name": "Votes", "autoWidth": true },
                { "data": "Views", "title": "Views", "name": "Views", "autoWidth": true },
                { "data": "Answers", "title": "Answers", "name": "Answers", "autoWidth": true },
                {
                    "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Home/Edit/' + full.id + '">Sửa</a>'; }
                },
                {
                    "render": function (data, type, row) {
                        return "<a href='#' class='btn btn-danger' " + ">Xóa</a>";
                    }
                },
            ]
        });
    })