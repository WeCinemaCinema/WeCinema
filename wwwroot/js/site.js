const dropdownContent = document.querySelector(".dropdown-content");

const dropdownIcon = document.querySelector(".svg-dropdown");

dropdownIcon.addEventListener("click", (e) => {
  dropdownContent.classList.toggle("show");
});

window.addEventListener("click", (event) => {
  if (!event.target.matches(".svg-dropdown")) {
    dropdownContent.classList.remove("show");
  }
});
