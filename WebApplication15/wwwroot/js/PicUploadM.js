$(".filebtn").bind("click",()=>{
    $("#input_file").click();
})


$("#input_file").on("change", () => {
    let file = document.getElementById("input_file").files[0];
    //let files=file.files;
    //console.log(files);

    //建立虚拟的form表单
    let picInForm = new FormData();

    let AuthorizationString="Bearer "+$.cookie("token");
    //将文件加入到formData
    picInForm.append(file.name, file);
    //console.log(fileObj1);
    //console.log(picInForm);
    $.ajax({
        headers:{
            "Authorization":AuthorizationString
        },
        type: "post",
        url: "api/Pic/upLoadPic",
        data: picInForm,
        //防止jquery对formdata预处理
        contentType: false,
        //contentType:"multipart/form-data; boundary=------",
        processData: false,
        //如果后台处理成功，执行success回调函数
        success: function (response) {
            console.log(response);
            window.location.href="/picAddInfo?pic="+response.fileName;
        },
        error: function(response){
                        //console.log(response);
                        
        }
    });

})