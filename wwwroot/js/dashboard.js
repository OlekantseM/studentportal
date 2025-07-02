document.addEventListener("DOMContentLoaded", function () {
    const tabButtons = document.querySelectorAll(".tab-button");
    const tabContents = document.querySelectorAll(".tab-content");

    tabButtons.forEach(button => {
        button.addEventListener("click", function () {
            const target = this.dataset.tab;

            tabButtons.forEach(btn => btn.classList.remove("active"));
            this.classList.add("active");

            tabContents.forEach(content => {
                content.style.display = content.id === target ? "block" : "none";
            });
        });
    });

    // Show the first tab by default
    const defaultTab = document.querySelector(".tab-button.active") || tabButtons[0];
    if (defaultTab) defaultTab.click();
});
