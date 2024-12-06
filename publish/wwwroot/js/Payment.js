document.querySelectorAll(".dropdown-box").forEach((box) => {
  box.addEventListener("click", (e) => {
    const isOpen = box.classList.contains("open");

    document
      .querySelectorAll(".dropdown-box")
      .forEach((b) => b.classList.remove("open"));

    if (!isOpen) {
      box.classList.add("open");
    }

    e.stopPropagation();
  });
});

document.querySelectorAll(".circle-button").forEach((button) => {
  button.addEventListener("click", (e) => {
    const buttonGroup = e.target.dataset.group;
    document
      .querySelectorAll(`[data-group="${buttonGroup}"]`)
      .forEach((btn) => btn.classList.remove("selected"));
    e.target.classList.add("selected");

    e.stopPropagation();
  });
});

document.addEventListener("click", () => {
  document
    .querySelectorAll(".dropdown-box")
    .forEach((box) => box.classList.remove("open"));
});
