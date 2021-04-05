var pageIndex = 1;
var pageSize = 10;
var Product = function () {
    return {
        init: function () {
            Product.loadData();
            //var keyWord = $('#txtSearch').val();
            //if (keyWord !== undefined) {
            //    Product.loadData();
            //}
            $('#txtSearch').keypress(function (e) {
                if (e.which === 13) {
                    Product.loadData();
                    return false;
                }
            });
        },
        loadData: function () {
            //var keyWord = $('#txtSearch').val();
            var keyWord = "";
            $.get("/Product/ListData", { keyWord: keyWord, pageIndex: pageIndex, pageSize: pageSize }, function (res) {
                $('#loadData').html(res.viewContent);
                if (res.totalPages > 1) {
                    $('#paginationholder').html('<ul id="pagination" class="pagination-sm"></ul>');
                    $('#pagination').twbsPagination({
                        startPage: self.pageIndex,
                        totalPages: res.totalPages,
                        visiblePages: 5,
                        onPageClick: function (event, page) {
                            self.pageIndex = page;
                            Product.loadData(self.pageIndex);
                        }
                    });
                } else {
                    $('#paginationholder').html('');
                }
            });
        } 
    }
}();
$(function () {
    Product.init();
})