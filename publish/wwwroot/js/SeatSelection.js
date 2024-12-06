document.addEventListener("DOMContentLoaded", () => {
  const seatsWrapper = document.getElementById("seats-wrapper");
  const rows = 10;
  const colsLeft = 3;
  const colsMiddle = 12;
  const colsRight = 3;

  const takenSeats = [
    "G2",
    "G3",
    "E1",
    "E2",
    "E3",
    "D6",
    "D7",
    "D8",
    "D9",
    "D10",
    "B7",
    "B8",
    "B9",
    "A6",
    "A7",
    "A8",
    "A9",
    "A10",
    "A11",
    "A12",
    "A13",
    "G9",
    "G10",
    "G11",
    "G12",
    "G13",
    "G18",
    "H17",
    "F16",
    "F17",
  ];

  for (let row = rows - 1; row >= 0; row--) {
    const rowLetter = String.fromCharCode(65 + row);

    const rowDiv = document.createElement("div");
    rowDiv.classList.add("seats-container-row");

    const leftLabel = document.createElement("div");
    leftLabel.classList.add("row-label");
    leftLabel.innerText = rowLetter;
    rowDiv.appendChild(leftLabel);

    const seatsContainer = document.createElement("div");
    seatsContainer.classList.add("seats-container");

    for (let col = 1; col <= colsLeft; col++) {
      createSeat(rowLetter, col, seatsContainer, takenSeats);
    }

    createSpacer(seatsContainer);

    for (let col = 4; col < 4 + colsMiddle; col++) {
      createSeat(rowLetter, col, seatsContainer, takenSeats);
    }

    createSpacer(seatsContainer);

    for (let col = 16; col < 16 + colsRight; col++) {
      createSeat(rowLetter, col, seatsContainer, takenSeats);
    }

    rowDiv.appendChild(seatsContainer);

    const rightLabel = document.createElement("div");
    rightLabel.classList.add("row-label");
    rightLabel.innerText = rowLetter;
    rowDiv.appendChild(rightLabel);

    seatsWrapper.appendChild(rowDiv);
  }

  function createSeat(row, col, container, takenSeats) {
    const seat = document.createElement("div");
    seat.classList.add("seat");
    seat.dataset.seatId = `${row}${col}`;
    if (takenSeats.includes(`${row}${col}`)) {
      seat.classList.add("taken");
    }

    seat.addEventListener("click", () => {
      if (!seat.classList.contains("taken")) {
        seat.classList.toggle("selected");
      }
    });
    container.appendChild(seat);
  }

  function createSpacer(container) {
    const spacer = document.createElement("div");
    spacer.classList.add("spacer");
    container.appendChild(spacer);
  }
});
