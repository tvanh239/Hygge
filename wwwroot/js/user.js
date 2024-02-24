var avaUser = document.getElementById("avatarImg");
var settingMenu = document.getElementById("settingMenu");

///---------------------Ready-------------------------
document.addEventListener('DOMContentLoaded', function () {
    // Check if the browser supports navigator.mediaDevices
    if (navigator.mediaDevices) {
        // Use enumerateDevices to get a list of media devices
        
         navigator.mediaDevices.enumerateDevices()
            .then(devices => {
                console.log(devices);
                // Filter audio devices
                const audioDevices = devices.filter(device => device.kind === 'audioinput');
                
                // If there are audio devices
                if (audioDevices.length > 0) {
                    selectElement = document.getElementById("micSetting");
                    audioDevices.forEach(function (option) {
                        var optionElement = document.createElement('option');
                        optionElement.value = removeLastParentheses(option.label);
                        optionElement.text = removeLastParentheses(option.label);
                        selectElement.add(optionElement);
                    });
                    
                } else {
                    console.error("No audio devices found.");
                }

                const speakDevices = devices.filter(device => device.kind === 'audiooutput');
                if (speakDevices.length > 0) {
                    speakSelect = document.getElementById("audioSetting");
                    speakDevices.forEach(function (option) {
                        var optionElement = document.createElement('option');
                        optionElement.value = removeLastParentheses(option.label);
                        optionElement.text = removeLastParentheses(option.label);
                        speakSelect.add(optionElement);
                    });

                } else {
                    console.error("No audio devices found.");
                }

            })

    } else {
        console.error("navigator.mediaDevices not supported in this browser.");
    }
});
///-----------------------------------------------------


// Function to remove the last set of parentheses and its content
function removeLastParentheses(inputString) {
    return inputString.replace(/\([^)]*\)$/, '').trim();
}


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