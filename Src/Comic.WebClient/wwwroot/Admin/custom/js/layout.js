async function getUserInformation() {
    if (window.sessionStorage.getItem("userId") == null) {
        $.ajax({
            url: `${ApiServer}/api/auth/get-infomation`,
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {},
            crossDomain: true,
            xhrFields: {
                withCredentials: true
            },
            success: async function (data) {
                window.sessionStorage.setItem("userId", data.data.id);
                window.sessionStorage.setItem("userName", data.data.name);
                window.sessionStorage.setItem("userEmail", data.data.email);
                window.sessionStorage.setItem("userAvatar", data.data.avatar);
                window.sessionStorage.setItem("userPhoneNumber", data.data.phoneNumber);
                window.sessionStorage.setItem("userRoles", JSON.stringify(data.data.roles));

                await showUserInformation();
            },
            error: async function (httpObject, error) {
                console.log(httpObject)
                const result = await handleErrorReponseAsync(httpObject);
                if (result == true) {
                    await getUserInformation();
                }
            }
        });
    } else {
        await showUserInformation();
    }
    

    
}

async function showUserInformation() {
    $("#auth-name").html(window.sessionStorage.getItem("userName"));
    var roles = JSON.parse(window.sessionStorage.getItem("userRoles"));
    $("#auth-roles").html("");
    roles.forEach((role) => {
        $("#auth-roles").append(role);
        console.log(role);
    });

    console.log(roles);
}

async function setUpMenu() {
    $("#menuDashboard").on("click", function () {
        window.location.href = `${ClientServer}/admin/dashboard`;
    });
    $("#menuRole").on("click", function () {
        window.location.href = `${ClientServer}/admin/role`;
    }); 
    $("#menuPermission").on("click", function () {
        window.location.href = `${ClientServer}/admin/permission`;
    });
    $("#menuUser").on("click", function () {
        window.location.href = `${ClientServer}/admin/user`;
    });
    $("#menuCategory").on("click", function () {
        window.location.href = `${ClientServer}/admin/category`;
    });
    $("#menuStory").on("click", function () {
        window.location.href = `${ClientServer}/admin/story`;
    });
    $("#menuSeo").on("click", function () {
        window.location.href = `${ClientServer}/admin/seo`;
    });
}

$(document).ready(async function () {
    await getUserInformation();
    await setUpMenu();
});