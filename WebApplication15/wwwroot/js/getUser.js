



let AuthorizationString = "Bearer " + $.cookie("token");
$.ajax({
    headers: {
        "Authorization": AuthorizationString
    },
    type: "get",
    //data:$.cookie("token"),
    url: "api/User/token",
    success: function (response) {
        console.log(response);
        $("#username").html(response.userName);
        $("#age").html(response.age);
        let gender='其他';
        switch (response.gender) {
            case 1:
                gender='男';
                break;
            case 2:
                gender='女';
                break;
            default:
                break;
        }
        $("#gender").html(gender);
        $("#like4me").html(response.likes);
    }
});

