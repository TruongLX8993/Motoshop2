var pageIndex = 1;
var pageSize = 10;
var about = function () {
    return {
        init: function () {
            about.loadData();
        },
        loadData: function () {
            $.get("/Admin/About/ListData", { pageIndex: pageIndex, pageSize: pageSize }, function (res) {
                $('#gridData').html(res.viewContent);
            });
        },
        loadfrmDetail: function (id) {
            modal.Render("/Admin/About/Detail/" + id, "Chi tiết hướng dẫn", "modal-lg");
        },
        loadfrmAdd: function () {
            modal.Render("/Admin/About/Add", "Thêm mới bài viết giới thiệu", "modal-lg");
        },
        loadfrmedit: function (id) {
            modal.Render("/Admin/About/Edit/" + id, "Cập nhật bài viết giới thiệu", "modal-lg");
        },
        ondelete: function (id) {
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
                    AjaxService.POST("/Admin/About/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
      
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                location.href = "admin/about";
            } else {
                alertmsg.error(res.Message);
            }
        },
    
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Message);
                location.href = "admin/about";
            } else {
                alertmsg.error(res.Message);
            }
        },
         
    };
}();
$(function () {
    about.init();
});
