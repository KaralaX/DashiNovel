document.addEventListener("DOMContentLoaded", function () {
    const toggleButton = document.getElementById("toggleButton");
    const targetElement = document.getElementById("targetElement");

    toggleButton.addEventListener("click", function () {
        if (targetElement.style.display === "none") {
            targetElement.style.display = "block";
        } else {
            targetElement.style.display = "none";
        }
    });
});