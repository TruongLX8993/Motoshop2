var pageSize = 10;
var pageIndex = 1;
var instruction = function () {
    return {
        init: function () {
            var keyWord = $('#txtSearch').val();
            if (keyWord != undefined) {
                instruction.loadData(pageIndex);
            }
            $("#txtSearch").keypress(function (e) {
                if (e.which === 13) {
                    instruction.loadData(pageIndex);
                    return false;
                }
            });
        },
       
        loadData: function (pageindex) {
            var keyWord = $('#txtSearch').val();
            $.get("/Admin/Instruction/ListData", { keyWord: keyWord, pageIndex: pageindex, pageSize: pageSize }, function (res) {
                $('#gridData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            instruction.loadData(self.pageIndex);
                        }
                    });
                } else {
                    $('#paginationholder').html('');
                }
            });
        },  

        onSearchSuccess: function (res) {
            var self = this;
            $('#paginationholder').html('');
            $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
            $("#gridData").html(res.viewContent);
            instruction.DataSearch = {
                Name: $("#frmSearch #Name").val(),
                LangCode: $("#frmSearch #LangCode").val(),
                PageElementId: $("#frmSearch #PageElementId").val()
            };
            if (res.totalPages > 1) {
                $('#pagination').twbsPagination({
                    startPage: 1,
                    totalPages: res.totalPages,
                    visiblePages: 5,
                    onPageClick: function (event, page) {
                        instruction.pageIndex = page;
                        instruction.loadData(page);
                    }
                });
            }
            self.FirstLoad = false;
            $("#loading").hide();
        },
       
        loadfrmDetail: function (id) {
            modal.Render("/Instruction/Detail/" + id, "Chi tiết hướng dẫn", "modal-lg");
        },
        loadfrmAdd: function () {
            modal.Render("/Instruction/Add", "Thêm mới tiêu đề hướng dẫn", "modal-lg");
        },
        loadfrmedit: function (id) {
            modal.Render("/Instruction/Edit/" + id, "Cập nhật tiêu đề hướng dẫn", "modal-lg");
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
                    AjaxService.POST("/Admin/Instruction/Delete", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
        onmultidelete: function () {
            var self = this;
            if ($("table tbody").find("input[type=checkbox]:checked").length == 0) {
                alertmsg.error("Bạn cần chọn ít nhất một danh mục cần xóa");
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
                        AjaxService.POST("/Admin/Instruction/DeleteAll", { lstid: $("#hdfID").val() }, function (res) {
                            self.pageIndex = 1;
                            self.loadData(self.pageIndex);
                            alertmsg.success(res.Messenger);
                        });
                    }
                    $("#loading").hide();
                });
            }
        },
        initcheckall: function () {
            var countall = $(".chkelement").length;
            var countchecked = 0;
            $(".chkelement").each(function () {
                if ($(this).find($("input[type=checkbox]")).is(':checked')) {
                    countchecked++;
                }
            });
            if (countall == countchecked) {
                $("#basicForm #chkall").prop('checked', true);
            } else {
                $("#basicForm #chkall").prop('checked', false);
            }
            $("#basicForm #chkall").click(function () {
                if ($(this).is(':checked')) {
                    $(".chkelement").each(function () {
                        $(this).find($("input[type=checkbox]")).prop('checked', true);
                    });
                } else {
                    $(".chkelement").each(function () {
                        $(this).find($("input[type=checkbox]")).prop('checked', false);
                    });
                }
            });
            $(".chkelement").click(function () {
                countchecked = 0;
                $(".chkelement").each(function () {
                    if ($(this).find($("input[type=checkbox]")).is(':checked')) {
                        countchecked++;
                    }
                });
                if (countall == countchecked) {
                    $("#basicForm #chkall").prop('checked', true);
                } else {
                    $("#basicForm #chkall").prop('checked', false);
                }
            });
        },
        initcheckall2: function () {
            var countall = $(".chkelement2").length;
            var countchecked = 0;
            $(".chkelement2").each(function () {
                if ($(this).find($("input[type=checkbox]")).is(':checked')) {
                    countchecked++;
                }
            });
            if (countall == countchecked) {
                $("#basicForm #chkall2").prop('checked', true);
            } else {
                $("#basicForm #chkall2").prop('checked', false);
            }
            $("#basicForm #chkall2").click(function () {
                if ($(this).is(':checked')) {
                    $(".chkelement2").each(function () {
                        $(this).find($("input[type=checkbox]")).prop('checked', true);
                    });
                } else {
                    $(".chkelement2").each(function () {
                        $(this).find($("input[type=checkbox]")).prop('checked', false);
                    });
                }
            });
            $(".chkelement2").click(function () {
                countchecked = 0;
                $(".chkelement2").each(function () {
                    if ($(this).find($("input[type=checkbox]")).is(':checked')) {
                        countchecked++;
                    }
                });
                if (countall == countchecked) {
                    $("#basicForm #chkall2").prop('checked', true);
                } else {
                    $("#basicForm #chkall2").prop('checked', false);
                }
            });
        },
        onAddSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                location.href = "/admin/instruction";
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        
        onEditSuccess: function (res) {
            if (res.IsSuccess == true) {
                alertmsg.success(res.Messenger);
                location.href = "/admin/instruction";
            } else {
                alertmsg.error(res.Messenger);
            }
            $("#loading").hide();
        },
        activeHH: function (id) {
            
            $("#loading").show();
            var self = this;
            swal({
                title: "Thay đổi trạng thái?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Có",
                cancelButtonText: "không",
            }, function (isConfirm) {
                if (isConfirm) {
                    AjaxService.POST("/Admin/Instruction/ChangeStatus", { id: id }, function (res) {
                        self.pageIndex = 1;
                        self.loadData(self.pageIndex);
                        alertmsg.success(res.Messenger);
                    });
                }
                $("#loading").hide();
            });
        },
    };
}();
$(function () {
    instruction.init();
});
