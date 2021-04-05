var cart = function () {
    return {
        init: function () {
            this.pageIndex = 1;
            if (typeof window.orientation !== 'undefined') {
                let total = 0;
                $(".thanhtienmobile").each(function () {
                    let tien = $(this).val();
                    total += parseInt(tien);
                });
                let numCommas = cart.addCommas(total);
                $('#totalMoneyMobile').text(numCommas);
            }
        },
        loadData: function (pageindex) {
            $.get("/contact/ListData?page=" + pageindex, null, function (res) {
                $("#qa-details").append(res.viewContent);
           
            });
        },
       
        changeQuantity: function (quantity, id) {
            if (quantity <= 0) {
                alert("Số lượng phải hơn hơn 0");
                $('#quantity_' + id).val(1);
            } else {
                let total = 0;
                let priceMoney = $('#hdPriceMoney_' + id).val().replace(',', '');
                let countMoney = parseInt(priceMoney) * parseInt(quantity);
                $('#hdTotalPriceMoney_' + id).val(countMoney);
                let thanhtien = cart.addCommas(countMoney);
                $('#totalPriceMoney_' + id).text(thanhtien);
                $(".total_price_money").each(function () {
                    let money = $(this).val().replace(',', '');
                    total += parseInt(money);
                });
                let numCommas = cart.addCommas(total);
                $('#totalMoney').text(numCommas);
            }
        },
        changeQuantityMobile: function (quantity, id) {
            if (quantity.trim() !== "") {
                if (quantity < 0) {
                    alert("Số lượng phải hơn hơn 0");
                    $('#quantityMobile_' + id).val(1);
                } else {
                    let total = 0;
                    let price = $('#hdPriceMoneyMobile_' + id).val().replace(',', '');
                    let money = parseInt(price) * parseInt(quantity);
                    $('#hdTotalPriceMoneyMobile_' + id).val(money);
                    let thanhtien = cart.addCommas(money);
                    $('#totalPriceMoneyMobile_' + id).text(thanhtien);
                    $(".thanhtienmobile").each(function () {
                        let tien = $(this).val().replace(',', '');
                        total += parseInt(tien);
                    });
                    let numCommas = cart.addCommas(total);
                    $('#totalMoneyMobile').text(numCommas);
                }
            }
        },
        onAddSuccess: function (res) {
            if (res.Success==true){
                alert("Đặt hàng thành công");
                localStorage.clear();
                location.href = "/trang-chu.html";
            } 
            else
                alert("Đặt hàng thất bại");
        },
        orderProduct: function (count) {
        if (count <= 0) {
            alert("Vui lòng chọn mua sản phẩm");
            return false;
        }
        let validate = true;
        let fullname = $('#fullname').val();
        if(fullname===''){
            $('#errorFullName').text("Vui lòng điền họ tên");
            validate = false;
        } 
        let address = $('#address').val();
        if(address===''){
            $('#errorAddress').text("Vui lòng nhập địa chỉ");
            validate = false;
        }
        let phone = $('#phone').val();
        if(phone===''){
            $('#errorPhone').text("Vui lòng nhập số điện thoại");
            validate = false;
        }
        var lstProduct = [];
        if (validate) {
            if (typeof window.orientation !== 'undefined') {
                for (var k = 1; k <= count; k++) {
                    let productid = $('#productidMobile_' + k).val();
                    let quantity = $('.quantityMobile_' + k).val();
                    var order = {
                        ProductID: productid,
                        Quantity: quantity
                    }
                    lstProduct.push(order);
                }
                var customer = {
                    CustomerName: fullname,
                    CustomerAddress: address,
                    CustomerPhone: phone,
                    CustomerEmail: $('#gmail').val()
                }
            } else {
                for (var k = 1; k <= count; k++) {
                    let productid = $('#productid_' + k).val();
                    let quantity = $('.quantityProduct_' + k).val();
                    var order = {
                        ProductID: productid,
                        Quantity: quantity
                    }
                    lstProduct.push(order);
                }
                var customer = {
                    CustomerName: fullname,
                    CustomerAddress: address,
                    CustomerPhone: phone,
                    CustomerEmail: $('#gmail').val()
                }
            }
            $.ajax({
                type: 'POST',
                url: '/Cart/Order/',
                dataType: 'json',
                data: {
                    customerOrder:JSON.stringify(customer),
                    orderDetail:JSON.stringify(lstProduct)
                },
                success: function (res) {
                    if (res.Success == true) {
                        localStorage.clear();
                        alert("Đặt hàng thành công");
                        location.href = "/trang-chu.html";
                    } 
                    else
                        alert("Đặt hàng thất bại");
                }
            });
        }
        },
        addCommas: function (nStr) {
            nStr += '';
            var comma = /,/g;
            nStr = nStr.replace(comma, '');
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        },
        cancelOrder: function () {
            localStorage.clear();
            alert("Đã hủy đơn hàng");
            location.href = "/trang-chu.html";
        },
        blurQuantity: function (quantity,id) {
            if (quantity <= 0) {
                alert("Số lượng phải lớn hơn 0");
                $('#quantityMobile_' + id).val(1);
                let price = $('#hdPriceMoneyMobile_' + id).val();
                let money = parseInt(price) * 1;
                let thanhtien = cart.addCommas(money);
                $('#totalPriceMoneyMobile_' + id).text(thanhtien);
            }
        }
    }
}();
$(function () { cart.init(); });


