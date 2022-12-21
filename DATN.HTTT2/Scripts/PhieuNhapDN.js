$(function () {
    $("#add").click(function () {
        var isValid = true;

        if (document.getElementById("hanghoa").value == '' || document.getElementById("dongia").value == '') {
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


        if (document.getElementById("thanhtien").value == '') {
            $('#thanhtien').siblings('span.error').text('Hãy kiểm tra lại');
            isValid = false;
        }
        else {
            $('#thanhtien').siblings('span.error').text('');
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
            var td6 = document.createElement("td");
            var td7 = document.createElement("td");
            var td7 = document.createElement("td");
            var td8 = document.createElement('td');


            td1.className = "hanghoa";
            td2.className = "dvtinh";
            td3.className = "soluong";
            td4.className = "dongia";
            td6.className = "thanhtien right-align";
            td7.className = "ghichu";
            td8.className = "remove btn btn-danger";


            td1.innerHTML = document.getElementById("hanghoa").value;
            td2.innerHTML = document.getElementById("dvtinh").value;
            td3.innerHTML = document.getElementById("soluong").value;
            td4.innerHTML = document.getElementById("dongia").value;
            td6.innerHTML = document.getElementById("thanhtien").value;
            td7.innerHTML = document.getElementById("ghichu").value;
            td8.innerHTML = 'Xóa';
            row.appendChild(td1); 
            row.appendChild(td2);
            row.appendChild(td3); 
            row.appendChild(td4);
            row.appendChild(td6);
            row.appendChild(td7); 
            row.appendChild(td8);

            //table.children[0].appendChild(row);
            $("#bangct tbody").append(row);
            var tong = parseInt($('#tongtien').text()) + parseInt(td6.innerHTML);
            $('#tongtien').text(tong);

            $('#hanghoa').val('');
            $("#dvtinh").val('');
            $("#soluong").val('');
            $("#dongia").val('');
            $("#thanhtien").val('');
            $("#ghichu").val('');
        }
    });

   
    $("#bangct").on("click", ".remove", function () {
        var tong2 = parseInt($('#tongtien').text()) - parseInt($(this).closest('tr').children('td.thanhtien').text());
        $('#tongtien').text(tong2);
        $(this).parents("tr").remove();
    });
    
    $("#submit").click(function () {
        var isValid = true;

        var itemsList = [];

        $("#bangct tbody tr").each(function () {
            var item = {
                hanghoa: $('.hanghoa', this).text(),
                soluong: $('.soluong', this).text(),
                thanhtien: $('.thanhtien', this).text(),
                ghichu: $('.ghichu', this).text()
            }
            itemsList.push(item);
        });

        if (itemsList.length == 0) {
            $('#orderMessage').text('Thêm chi tiết phiếu nhập!');
            isValid = false;
        }
        if (document.getElementById("nhacc").value == '') {
            $('#orderMessage').text('Kiểm tra thông tin nhà cung cấp');
            isValid = false;
        }

        if (isValid) {
            var data = {
                nhacc: $("#nhacc").val(),
                danhsachct: itemsList
            }

            $("#submit").attr("disabled", true);
            $("#submit").html('Đang lưu phiếu nhập hàng ...');

            $.ajax({
                type: 'POST',
                url: '/PNHCt/Create',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Lưu phiếu nhập hàng');
                        setTimeout(function () { window.location.reload(); }, 2000);
                    }
                    else {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Lưu phiếu nhập hàng');
                    }
                }
            });
        }
    });


    $("#submit2").click(function () {
        var data = {
            mapn: $('#madh').html(),
            data12: document.getElementById('bangct2').innerHTML
        }

            $("#submit2").attr("disabled", true);
            $("#submit2").html('Đang gửi phiếu nhập hàng ...');

            $.ajax({
                type: 'POST',
                url: '/PNHCt/Create2',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Gửi');
                        setTimeout(function () { window.location.reload(); }, 1500);
                    }
                    else {
                        $('#orderMessage').text(data.message);
                        $("#submit").attr("disabled", false);
                        $("#submit").html('Gửi');
                    }
                }
            });
        
    });
});





