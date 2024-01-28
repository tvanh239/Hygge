var avaUser = document.getElementById("avatarImg");
avaUser.addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.showModal();
    document.addEventListener('click', handleClickOutside);
});

document.getElementById("closeInfo").addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.close();
});

function handleClickOutside(event) {
    var dialogInfo = document.getElementById("personalInfo");
    if (!dialogInfo.contains(event.target) && event.target !== document.getElementById('dialog') && event.target !== document.getElementsByClassName("image-avatar")[0]) {
        // Click is outside the dialog, so close it
        dialogInfo.close();
    }
}
