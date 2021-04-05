var magazine = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            this.FirstLoad = true;
            this.DataSearch = {};
        },
        loadData: function (pageindex) {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/tapchi/ListData?page=" + pageindex, magazine.DataSearch, function (res) {
                $("#gridData").html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            magazine.loadData(self.pageIndex);
                        }
                    });
                }
                self.FirstLoad = false;
                $("#loading").hide();
            });
        },
        loadDataContent: function (id, pageindex) {
            var self = this;
            AjaxService.GET("/tapchi/GetContent?page=" + pageindex + "&id=" + id, magazine.DataSearch, function (res) {
                $("#gridDataContent").html(res);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('');
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            magazine.loadDataContent(id, self.pageIndex);
                        }
                    });
                }
            });
        },
        onAddPageSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                magazine.loadfrmcontent(res.Magazineid);
            } else {
                alertmsg.error(res.Messenger);
            }
        },
        onSearchSuccess: function (res) {
            var self = this;
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            magazine.DataSearch = {
                Name: $("#Name").val(),
                LangCode: $("#LangCode").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        magazine.pageIndex = page;
                        magazine.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
        loadfrmAdd: function () {
            modal.Render("/tapchi/Add", "Thêm mới cơ quan ban hành", "modal-lg");
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                magazine.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmedit: function (id) {
            modal.Render("/tapchi/Edit/" + id, "Cập nhật cơ quan ban hành", "modal-lg");
        },
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                magazine.loadData(this.pageIndex);
                modal.hide();
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        loadfrmcontent: function (id) {
            modal.Render("/tapchi/AddContent/" + id, "Nội dung của trang tạp chí", "modal-lg");
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
                    AjaxService.POST("/tapchi/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        ondeletecontent: function (id, magazineid) {
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
                    AjaxService.POST("/tapchi/DeleteContent", { id: id, magazineid: magazineid }, function (res) {
                        self.pageIndex = 1;
                        self.loadfrmcontent(magazineid);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một cơ quan ban hành cần xóa");
            } else {
                $("#loading").show();
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
                        AjaxService.POST("/tapchi/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        onupdateposittion: function (magazineid) {
            var self = this;
            var arrValue = [];
            $(".lstContent .ordering").each(function () {
                var id = $(this).find("#item_ID").val();
                var ordering = $(this).find("input[type=text]").val();
                var str = id + ":" + ordering;
                arrValue.push(str);
            });
            var strValue = arrValue.join("|");
            AjaxService.POST("/tapchi/UpdatePosition", { value: strValue, magazineid: magazineid }, function (res) {
                if (res.IsSuccess == true) {
                    alertmsg.success(res.Messenger);
                } else {
                    alertmsg.error(res.Messenger);
                }
                self.loadfrmcontent(magazineid);
            });
        }
    };
}();
$(function () { magazine.init(); });


