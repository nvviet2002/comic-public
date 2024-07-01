$(document).ready(async function () {
    //click description more
    $("#storyMoreDescription").on("click", async function (e) {
        e.preventDefault();
        $("#storyDescription").removeClass("shorted");
        $(this).addClass("d-none");
    });

    //click chapters more
    $("#storyMoreChapter").on("click", async function (e) {
        e.preventDefault();
        $("#storyChapters").removeClass("shorted");
        $(this).addClass("d-none");
    });
  
});