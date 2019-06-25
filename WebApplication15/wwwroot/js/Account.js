class user {

    constructor(UserName, Password, Gender, Age) {
        this.UserName = UserName;
        this.Password = Password;
        this.Gender = Gender;
        this.Age = Age;
    }
}
    document.getElementById('repass').onfocus = () => {
        $("#RegErro").hide();
    };
    document.getElementById('username').onfocus = () => {
        $("#RegErro").hide();
    };

$('#reg').bind("click", () => {
    let u = new user($("#username").val(), $("#newpassword").val(), $('input[name="sex"]:checked').val(), $("#age").val());
    let repass = $("#repass").val();

    if (u.UserName === "") {
        $("#RegErro").children("strong").html("请输入用户名");
        $("#RegErro").show();
        return;
    }

    if (u.Password.length < 6) {
        $("#RegErro").children("strong").html("密码至少6位");
        $("#RegErro").show();
        return;
    }
    if (u.Password === repass) {
        console.log(repass);
        console.log(u);
        $.ajax({
            async: false,
            type: "post",
            url: "api/User/register",
            data: JSON.stringify(u),
            //data:data,
            contentType: 'application/json; charset=utf-8',
            // dataType: "json",
            success: function (response) {
                if (response.status != 1) {
                    console.log("已存在");
                    $("#username").val("");
                    $("#RegErro").children("strong").html("用户名已存在");
                    $("#RegErro").show();
                } else {
                    $("#username").val("");
                    $("#newpassword").val("");
                    $("#repass").val("");
                    $("#age").val("0");
                    $('input:radio:last').attr('checked', 'checked');
                    $("#close").click();
                }
            }
        });
    } else {
        $("#RegErro").children("strong").html("两次密码输入不一致");
        $("#RegErro").show();
        return;
    }
   
    /**/
});