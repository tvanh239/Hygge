var avaUser = document.getElementById("avatarImg");

avaUser.addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.showModal();
});

document.getElementById("closeInfo").addEventListener("click", function () {
    var dialogInfo = document.getElementById("personalInfo");
    dialogInfo.close();
});