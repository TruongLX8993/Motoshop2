var pageIndex = 1;
var pageSize = 10;
var InfoUseful = function () {
    return {
        init: function () {
            InfoUseful.loadData();
            $("#txtSearch").keypress(function (e) {
                if (e.which === 13) {
                    InfoUseful.loadData();
                    return false;
                }
            });
        },
        loadData: function () {
            var page = localStorage.getItem("PAGE");
            if (page === null)
                page = pageIndex;
            $.get("/Admin/InfoUseful/ListData", {page: page}, function (res) {
                $('#loadData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            InfoUseful.loadData(self.pageIndex);
                        }
                    });
                } else {
                    $('#paginationholder').html('');
                }
            });
        }, 
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                if (res.Close == "close") {
                    let url = "/Admin/InfoUseful";
                    location.href = url;
                } else {
                    location.href = "/Admin/InfoUseful/add";
                }
            }
            else {
                alertmsg.error(res.Message);
            }
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                localStorage.setItem("PAGE", res.Page);
                let url = "/Admin/InfoUseful";
                location.href = url;
            }
            else {
                alertmsg.error(res.Message);
            }
        },
        handleDeleteItem: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có chắc chắn không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/InfoUseful/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        handlePublish: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn đăng bài?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/InfoUseful/Publish", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        handleUnPublish: function (id) {
            swal({
                title: "Bạn có muốn hủy đăng bài?",
                type: "warning",
                showCancelButton: !0,
                confirmButtonClass: "btn-warning",
                confirmButtonText: "Đồng ý!",
                closeOnConfirm: !1
            }, function () {
                $.get("/Admin/InfoUseful/UnPublish/", { id: id }, function (res) {
                    swal.close();
                    if (res.IsSuccess == true)
                        alertmsg.success(res.Message);
                    else
                        alertmsg.error(res.Message);
                    InfoUseful.loadData();
                })
            })
        }
    }
}();
$(function () {
    InfoUseful.init();
})