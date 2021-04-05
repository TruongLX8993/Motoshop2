var languages = function () {
    return {
        init: function () {
            this.FirstLoad = true;
        },
        loadData: function () {
            var self = this;
            $("#loading").show();
            AjaxService.POST("/languages/ListData", null, function (res) {
                $("#gridData").html(res.viewContent);
                self.FirstLoad = false;
                $("#loading").hide();
            });
        }
    };
}();
$(function () { languages.init(); });


