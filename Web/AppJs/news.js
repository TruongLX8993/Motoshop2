var pageIndex = 1;
var pageSize = 10;
var News = function () {
    return {
        init: function () {
            var keyWord = $('#txtSearch').val();
            if (keyWord != undefined) {
                News.loadData();
            }
            $("#txtSearch").keypress(function (e) {
                if (e.which === 13) {
                    News.loadData();
                    return false;
                }
            });
        },
        loadData: function () {
            var keyWord = $('#txtSearch').val();
            $.get("/Admin/News/ListData", { keyWord: keyWord, pageIndex: pageIndex, pageSize: pageSize }, function (res) {
                $('#loadData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            News.loadData(self.pageIndex);
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
                    let url = "/admin/news";
                    location.href = url;
                } else {
                    location.href = "/admin/news/add";
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
                let url = "/admin/news";
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
                title: "Bạn có chắc chắn xóa không?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.GET("/Admin/News/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Message);
                    });
                }
                $("#loading").hide();
            });
        },
        handlePublish: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn đăng bài viết?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.GET("/Admin/News/Publish/", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Message);
                    });
                }
                $("#loading").hide();
            });
        },
        handleUnPublish: function (id) {
            $("#loading").show();
            var self = this;
            swal({
                title: "Bạn có muốn hủy đăng bài viết?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.GET("/Admin/News/UnPublish/", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Message);
                    });
                }
                $("#loading").hide();
            });
        }
    }
}();
$(function () {
    News.init();
})