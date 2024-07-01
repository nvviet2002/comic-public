$(document).ready(async function () {

    await initCarousel();
    //search story on header
    await initStorySearchInput();
   
    await initNavCategory();

});
//carousel
async function initCarousel() {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: true,
        autoplayTimeout: 4000,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            360: {
                items: 2,
                nav: false
            },
            576: {
                items: 2,
                nav: false
            },
            768: {
                items: 3,
                nav: false
            },
            992: {
                items: 4,
                nav: false
            },
            1200: {
                items: 5,
                nav: false
            }
        }
    });
}
//carousel
//story search
async function initStorySearchInput() {
    //setup before functions
    var typingTimer;                //timer identifier
    var doneTypingInterval = 1000;  //time in ms, 5 seconds for example
    //on keyup, start the countdown
    $("#headerSearchInput").on('keyup change paste', async function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(await searchStoryAjaxAsync, doneTypingInterval);
    });

    //on keydown, clear the countdown 
    $("#headerSearchInput").on('keydown', async function () {
        clearTimeout(typingTimer);
    });

    //on focus
    $("#headerSearchInput").on('focus', async function () {
        $("#headerSearchContainer").removeClass("d-none");
    });

    //on click body to hide search result
    $("body").on('click', async function (event) {

        if (!$(event.target).closest('#headerSearchContainer').length && !$(event.target).closest('#headerSearchInput').length) {
            $("#headerSearchContainer").addClass("d-none");
        }
        
    });
    //search when user typed enter key
    $("#headerSearchInput").on('search', function () {
        navigationSearchStory();
    });
}

async function searchStoryAjaxAsync() {
    var searchTerm = $(headerSearchInput).val();
    $.ajax({
        url: `${ApiServer}/api/story/search-paging`,
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: {
            "PaginateReq.PageSize": 10,
            "PaginateReq.PageNumber": 1,
            "PaginateReq.SearchTerm": searchTerm,
        },
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        success: async function (data) {
            console.log(data);
            showSearchContainer(data.data.items);
        },
        error: async function (httpObject, error) {
            const result = await handleErrorReponseAsync(httpObject);
            if (result == true) {
                await searchStoryAsync();
            }
        }
    });
}
function showSearchContainer(items) {
    var html = "";
    items.forEach(function (story) {
        var chapterText = "";
        if (story.chapters.length <= 0) {
            chapterText = `Đang cập nhật`;
        } else {
            chapterText = story.chapters[0].name;
        }
       
        html += `<a href="/truyen-tranh/${story.slug}/${story.id}" class="row p-2 header-search-item">
                    <div class="col-2">
                        <div class="main-card-image">
                            <img src="${story.avatar}" alt="${story.name}" />
                        </div>
                    </div>
                    <div class="col-10 p-1">
                        <p class="fs-15">${story.name}</p>
                        <span class="fs-13">${chapterText}</span>
                    </div>
                </a>`;
    });
    $("#headerSearchContainer").html(html);

}
function navigationSearchStory() {
    var searchTerm = $(headerSearchInput).val();
    window.location.href = `${ClientServer}/truyen-tranh/the-loai/tat-ca?searchTerm=${searchTerm}`;
    
}
//story search
async function initNavCategory() {
    $(".nav-category-item").on("mouseover",function (e) {
        e.preventDefault();
        $("#navCategoryDescription").html($(this).data("desc"));
    });
}