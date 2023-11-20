var txtUID = document.getElementById('txtUID').value;

$(document).ready(function () {
    $('button').click(function () {
        bindTableUserAddress();
    });
});
var apiBaseUrl = "https://localhost:7025";
function bindTableUserAddress() {
    debugger;
    var tblUserAddress = $('#tblbodycontent');
    $.ajax({
        type: "GET",
        url: apiBaseUrl + "/api/User/GetUserProfileWithAddress" + txtUID + "/",
        //data: '{}'
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSeend: function () {
            $('.page-loader').show();
        },
        success: function (response) {
            /*console.log(response)
            console.log(response.statusCode)*/
            tblUserAddress.append('');
            var count = 1;
            $.each(response.data, function () {
                var htmlContent = `'<tr>
                 <td>`+ count + `</td>
					<td>`+ this['address'] + `</td>
                        <td>`+ this['city'] + `</td>
                        <td>`+ this['state'] + `</td>
                        <td>`+ this['country'] + `</td>
                        <td>`+ this['pin'] + `</td>
                    </tr>'`
                tblUserAddress.append(htmlContent);
                count++;
            });
            $('.page-loader').hide();
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}