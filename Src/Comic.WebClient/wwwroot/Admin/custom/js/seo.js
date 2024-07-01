
$(document).ready(async function () {
    $("#createSitemapBtn").on("click", async function (e) {
        e.preventDefault();
        await createSitemapAsync();
    });
   

});

//create sitemap
async function createSitemapAsync() {
    $.ajax({
        url: `${ClientServer}/admin/seo/create-sitemap`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {},
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        beforeSend: function () {
            blockPage();
        },
        success: async function (data) {
            toastr.success(data.message);
        },
        complete: function () {
            unblockPage();
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await createSitemapAsync();
            }
        }
    });
}
