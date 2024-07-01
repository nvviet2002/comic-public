$(document).ready(async function () {
    //click description more
    $("#chapterSelector").on("change", async function (e) {
        e.preventDefault();
        console.log($(this).find(":selected").val());
        window.location.href = $(this).find(":selected").val();
    });

    

});