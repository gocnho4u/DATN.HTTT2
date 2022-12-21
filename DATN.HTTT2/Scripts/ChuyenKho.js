$(function () {
    $("#add").click(function () {
        var isValid = true;

        if (document.getElementById("hanghoa").value == '') {
            $('#hanghoa').siblings('span.error').text('Hãy chọn hàng hóa');
            isValid = false;
        }
        else {
            $('#hangoa').siblings('span.error').text('');
        }

        if (document.getElementById("soluong").value == '') {
            $('#soluong').siblings('span.error').text('Kiểm tra lại số lượng!!!');
            isValid = false;
        }
        else {
            $('#soluong').siblings('span.error').text('');
        }

        if (isValid) {

            //var total = parseInt($("#quantity").val()) * parseFloat($("#price").val());
            //$("#total").val(total);

            //var ProductID = document.getElementById("productsItems").value;

            //var $newRow = $("#MainRow").clone().removeAttr('id');



            ////$('.productsItems', $newRow).val(ProductID);

            //$('#add', $newRow).addClass('remove').html('Remove').removeClass('btn-success').addClass('btn-danger');

            //$('#hanghoa, #dvtinh, #dongia, #soluong, #chietkhau, #thanhtien, #ghichu', $newRow).attr('disabled', true);

            //$('#hanghoa, #dvtinh, #dongia, #soluong, #chietkhau, #thanhtien, #ghichu', $newRow).removeAttr("id");
            //$("span.error", $newRow).remove(); 

            //$("#OrderItems").append($newRow);

            //var table = document.getElementById("OrderItems");

            var row = document.createElement("tr");
            //console.log(row);
            var td1 = document.createElement("td");
            var td2 = document.createElement("td");
            var td3 = document.createElement("td");
            var td4 = document.createElement("td");
            var td5 = document.createElement("td");


            td1.className = "hanghoa";
            td2.className = "dvtinh";
            td3.className = "soluong";
            td4.className = "ghichu";
            td5.className = "remove btn btn-danger";


            td1.innerHTML = document.getElementById("hanghoa").value;
            td2.innerHTML = document.getElementById("dvtinh").value;
            td3.innerHTML = document.getElementById("soluong").value;
            td4.innerHTML = document.getElementById("ghichu").value;
            td5.innerHTML = 'Xóa';
            row.appendChild(td1);
            row.appendChild(td2);
            row.appendChild(td3);
            row.appendChild(td4);
            row.appendChild(td5);


            //table.children[0].appendChild(row);
            $("#bangct tbody").append(row);


            $('#hanghoa').val('');
            $("#dvtinh").val('');
            $("#soluong").val('');
            $("#ghichu").val('');
        }
    });

    $("#bangct").on("click", ".remove", function () {
        $(this).parents("tr").remove();
    });

    $("#submit").click(function () {
        var isValid = true;
        if (document.getElementById("MaKHN").value == document.getElementById("MaKHX").value) {
            $('#loikho').text('Kho xuất và kho nhận phải khác nhau!!!');
            isValid = false;
        }
        else {
            $('#MaKHX').siblings('span.error').text('');
        }

        var itemsList = [];

        $("#bangct tbody tr").each(function () {
            var item = {
                hanghoa: $('.hanghoa', this).html(),
                SoLuong: $('.soluong', this).html(),
                ghichu: $('.ghichu', this).html()
            }
            itemsList.push(item);
        });

        if (itemsList.length == 0) {
            $('#orderMessage').text('Thêm chi tiết chuyển kho!');
            isValid = false;
        }
        

        if (isValid) {
            var data = {
                MaKHN: $("#MaKHN").val(),
                MaKHX: $("#MaKHX").val(),
                ctck: itemsList
            }

            $("#submit").attr("disabled", true);
            $("#submit").html('Đang lưu phiếu chuyển kho ...');

            $.ajax({
                type: 'POST',
                url: '/PCKCt/Create',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Lưu phiếu chuyển kho');
                        setTimeout(function () { location.reload(); }, 1500);
                    }
                    else {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Lưu phiếu chuyển kho');
                    }
                }
            });
        }
    });

});





