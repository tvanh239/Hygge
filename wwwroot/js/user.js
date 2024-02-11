var avaUser = document.getElementById("avatarImg");
var settingMenu = document.getElementById("settingMenu");

///----------------------------- Dialog for user avatar　↓----------------------------

avaUser.addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.showModal();
});

document.getElementById("closeInfo").addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.close();
});

function handleClickOutside(event) {
    var dialogInfo = document.getElementById("personalInfo");
    if (!dialogInfo.contains(event.target) && event.target != document.getElementsByClassName("image-avatar")[0]) {
        // Click is outside the dialog, so close it
        dialogInfo.close();
    }
    var menuDialog = document.getElementById("menuIcon");
    if (!menuDialog.contains(event.target) && event.target != document.getElementsByClassName("btn-setting")[0]) {
        // Click is outside the dialog, so close it
        settingMenu.classList.remove('card-active') ;
        menuDialog.close();
    }
}

///-------------------------------Dialog for user avatar　↑------------------------------



///----------------------------- Dialog for menu icon ↓--------------------------------
settingMenu.addEventListener("click", function () {
    this.className = this.className + ' card-active';
    var dialogInfo = document.getElementById("menuIcon");
    CreateMenuIcons();
    dialogInfo.showModal();
    
});
function CreateMenuIcons() {

}

document.addEventListener('click', handleClickOutside);
///------------------------------Dialog for menu icon ↑--------------------------------