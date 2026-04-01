function initPasswordToggles() {
    const toggles = document.querySelectorAll("[data-password-toggle]");

    toggles.forEach(function (button) {
        button.addEventListener("click", function () {
            const wrapper = button.closest(".password-toggle");
            if (!wrapper) return;

            const input = wrapper.querySelector("input");
            const icon = button.querySelector("i");

            if (!input) return;

            const isPassword = input.type === "password";
            input.type = isPassword ? "text" : "password";

            if (icon) {
                icon.className = isPassword ? "bi bi-eye-slash" : "bi bi-eye";
            }

            button.setAttribute("aria-label", isPassword ? "Jelszó elrejtése" : "Jelszó megjelenítése");
        });
    });
}

function initDropzones() {
    const zones = document.querySelectorAll("[data-dropzone]");

    zones.forEach(function (zone) {
        const inputSelector = zone.getAttribute("data-input");
        const listSelector = zone.getAttribute("data-list");

        if (!inputSelector || !listSelector) return;

        const input = document.querySelector(inputSelector);
        const list = document.querySelector(listSelector);

        if (!input || !list) return;

        function renderFiles(files) {
            list.innerHTML = "";
            if (!files || files.length === 0) {
                list.innerHTML = "<div>Nincs kiválasztott fájl.</div>";
                return;
            }

            Array.from(files).forEach(function (file) {
                const row = document.createElement("div");
                row.textContent = file.name;
                list.appendChild(row);
            });
        }

        zone.addEventListener("click", function () {
            input.click();
        });

        zone.addEventListener("dragover", function (e) {
            e.preventDefault();
            zone.classList.add("dragover");
        });

        zone.addEventListener("dragleave", function () {
            zone.classList.remove("dragover");
        });

        zone.addEventListener("drop", function (e) {
            e.preventDefault();
            zone.classList.remove("dragover");

            if (!e.dataTransfer || !e.dataTransfer.files || e.dataTransfer.files.length === 0) return;

            input.files = e.dataTransfer.files;
            renderFiles(input.files);
        });

        input.addEventListener("change", function () {
            renderFiles(input.files);
        });

        renderFiles(input.files);
    });
}

document.addEventListener("DOMContentLoaded", function () {
    initPasswordToggles();
    initDropzones();
});