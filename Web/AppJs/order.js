var pageIndex = 1;
var Order = function () {
    return {
        init: function () {
            this.FirstLoad = true;
            Order.loadData();
        },
        loadData: function () {
            var self = this;
            if (this.FirstLoad) {
                $("#loading").show();
            }
            let name = $('#customerName').val().trim();
            let status = $('#status').val();
            let tungay = $('#tuNgay').val();
            //if (tungay != '') {
            //    let arr = tungay.split('/');
            //    tungay = arr.reverse().join('/');
            //}
            let denngay = $('#denNgay').val();
            //if (denngay != '') {
            //    let arr = denngay.split('/');
            //    denngay = arr.reverse().join('/');
            //}
            AjaxService.POST("/Admin/Order/ListData", { status: status,name:name,tungay:tungay,denngay:denngay, page: pageIndex }, function (res) {
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            Order.pageIndex = page;
                            Order.loadData(page);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                Order.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
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
                    AjaxService.POST("/Admin/Order/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        viewDetail: function(id){
            modal.Render("/Admin/Order/Edit/" + id, "Cập nhật người dùng", "modal-lg");
      }
    };
}();
$(function () { Order.init(); });


