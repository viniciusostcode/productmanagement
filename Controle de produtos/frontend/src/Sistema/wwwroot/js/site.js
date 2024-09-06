var usernamesession = document.getElementById("user").dataset.username;
console.log(usernamesession);

localStorage.setItem('username', usernamesession);

var username = localStorage.getItem('username');
console.log(username);


const checkboxList = [];

function closeAddModal() {
    const modal = document.getElementById("myModal");
    modal.style.display = "none";
}

function closeEditModal() {
    const modal = document.getElementById("edit-modal");
    modal.style.display = "none";
}

fetch(`https://localhost:7215/products/find/${username}`)
    .then((response) => {
        if (!response.ok) {
            throw new Error("An error ocurred");
        }
        return response.json();

    })
    .then((data) => {
        console.log("Json data:", data);


        const tabela = document.getElementById("dataTable");

        const tbody = tabela.getElementsByTagName("tbody")[0];

        const noProductsMessage = document.getElementById("noProductsMessage");

        if (data.length === 0) {
            tabela.style.display = "none";
            noProductsMessage.style.display = "block";
            return;
        }

        tbody.innerHTML = "";

        data.forEach((item) => {

            const newRow = tbody.insertRow();

            const cell1 = newRow.insertCell(0);
            const cell2 = newRow.insertCell(1);
            const cell3 = newRow.insertCell(2);
            const cell4 = newRow.insertCell(3);
            const cell5 = newRow.insertCell(4);
            const cell6 = newRow.insertCell(5);
            const cell7 = newRow.insertCell(6);

            const date = new Date(item.date);

            const formatOptions = {
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit",
            };

            const formatDate = new Intl.DateTimeFormat("pt-BR", formatOptions);
            const FormatedDate = formatDate.format(date);

            cell1.textContent = "#" + item.id;
            cell2.textContent = item.product;
            cell3.textContent = FormatedDate;
            cell4.textContent = item.situation;
            cell5.textContent = item.quantity;
            cell6.textContent = item.price;
            cell7.textContent = username;

            const checkbox = document.createElement("input");

            checkbox.type = "checkbox";
            checkbox.className = "checkbox";
            checkbox.dataset.id = item.id;
            checkbox.style.display = "none";

            cell1.insertBefore(checkbox, cell1.firstChild);

            checkboxList.push(checkbox);
        });
    })
    .catch((error) => {
        console.error("An error ocurred:", error);
    });


document.addEventListener("DOMContentLoaded", function () {
    const checkedIds = [];

    const removeBtn = document.getElementById("removeBtn");

    const modalRemove = document.getElementById("remove-modal");

    const modalEdit = document.getElementById("edit-modal");

    const editBtn = document.getElementById("editBtn");

    const closeModalEditBtn = document.getElementById("closeModalEditBtn");

    const cancelModalEditBtn = document.getElementById("cancelModalEditBtn");

    closeModalEditBtn.addEventListener("click", function () {
        modalEdit.style.display = "none";
    });

    cancelModalEditBtn.addEventListener("click", function () {
        modalEdit.style.display = "none";
    });

    const closeModalRemovetBtn = document.getElementById("closeModalRemoveBtn");

    const cancelModalRemoveBtn = document.getElementById("cancelModalRemoveBtn");

    closeModalRemovetBtn.addEventListener("click", function () {
        modalRemove.style.display = "none";
    });

    cancelModalRemoveBtn.addEventListener("click", function () {
        modalRemove.style.display = "none";
    });

    removeBtn.addEventListener("click", function () {
        checkboxList.forEach((checkbox) => {
            checkbox.style.display = "inline-block";
        });
    });

    removeBtn.addEventListener("click", function () {
        checkboxList.forEach((checkbox) => {
            if (checkbox.checked) {
                checkedIds.push(checkbox.dataset.id);

                const row = checkbox.closest("tr");
                const cells = row.cells;

                modalRemove.classList.add("show");
                modalRemove.style.display = "block";

                const id = cells[0].textContent.replace("#", "");

                const saveChangesRemoveBtn = modalRemove.querySelector(
                    "#saveChangesRemoveBtn"
                );

                saveChangesRemoveBtn.addEventListener("click", function () {
                    const apiUrl = `https://localhost:7215/products/${id}`;

                    fetch(apiUrl, {
                        method: "DELETE",
                        headers: {
                            "Content-Type": "application/json",
                        },
                    })
                        .then((response) => response.json())
                        .then((data) => {
                            alert("product deleted with sucess!");
                            window.location.href = `https://localhost:7097/products`
                            closeEditModal();
                        })
                        .catch((error) => {
                            console.error("An error ocurred while deleting the product, check out the logs.", error);
                        });
                });
            }
        });
    });

    editBtn.addEventListener("click", function () {
        checkboxList.forEach((checkbox) => {
            checkbox.style.display = "inline-block";
        });
    });

    editBtn.addEventListener("click", function () {
        checkboxList.forEach((checkbox) => {
            if (checkbox.checked) {
                checkedIds.push(checkbox.dataset.id);

                const row = checkbox.closest("tr");
                const cells = row.cells;

                const id = cells[0].textContent.replace("#", "");
                const product = cells[1].textContent;
                const date = cells[2].textContent;
                const situation = cells[3].textContent;
                const quantity = cells[4].textContent;
                const price = cells[5].textContent;

                const modalEditContent = `

            <form id="editForm">
    <div class="mb-3">
      <b><label for="editProduct" class="form-label">Product:</label></b>
      <input type="text" class="form-control" id="editProduct" value="${product}">
    </div>
    <div class="mb-3">
      <b><label for="editDate" class="form-label">Date:</label></b>
      <input type="text" class="form-control" id="editDate" value="${date}">
    </div>
    <div class="mb-3">
      <b><label for="editSituation" class="form-label">Situation:</label></b>
      <input type="text" class="form-control" id="editSituation" value="${situation}">
    </div>
    <div class="mb-3">
      <b><label for="editQuantity" class="form-label">Quantity:</label></b>
      <input type="text" class="form-control" id="editQuantity" value="${quantity}">
    </div>
    <div class="mb-3">
      <b><label for="editPreco" class="form-label">Price:</label></b>
      <input type="text" class="form-control" id="editPrice" value="${price}">
    </div>
  </form>
          `;

                modalEdit.querySelector(".modal-body").innerHTML = modalEditContent;
                modalEdit.classList.add("show");
                modalEdit.style.display = "block";

                const saveChangesEditBtn = modalEdit.querySelector(
                    "#saveChangesEditBtn"
                );

                saveChangesEditBtn.addEventListener("click", function () {
                    const editedProduct = document.getElementById("editProduct").value;
                    const editedDate = document.getElementById("editDate").value;
                    const editedProductSituation =
                        document.getElementById("editSituation").value;
                    const editedQuantity = parseFloat(
                        document.getElementById("editQuantity").value
                    );
                    const editedPrice = document.getElementById("editPrice").value;

                    const editedData = {
                        price: editedPrice,
                        situation: editedProductSituation,
                        quantity: editedQuantity,
                        date: new Date(editedDate),
                        product: editedProduct,
                    };

                    const apiUrl = `https://localhost:7215/products/${id}`;

                    fetch(apiUrl, {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(editedData),
                    })
                        .then((response) => {

                            if (!response.ok) {
                                throw new Error(`HTTP error! Status: ${response.status}`);
                            }

                            return response.json();
                        })

                        .then((data) => {
                            alert("product updated with sucess!");
                            window.location.href = window.location.href = `https://localhost:7097/Products`
                        })
                        .catch((error) => {
                            console.error("Erro:", error);
                            alert("An erro ocurred while updating a product, check out the logs");
                        });

                    modalEdit.style.display = "none";
                });
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        const rows = document.querySelectorAll("#dataTable tbody tr");
        rows.forEach(function (row) {
            row.classList.add("fadeIn");
        });
    }, 100);
});

document.addEventListener("DOMContentLoaded", function () {
    const openModalBtn = document.getElementById("openModalBtn");
    const closeModalBtn = document.getElementById("closeModalAddBtn");
    const cancelModalBtn = document.getElementById("cancelModalAddBtn");
    const modal = document.getElementById("myModal");

    openModalBtn.addEventListener("click", function () {
        modal.style.display = "block";
    });

    closeModalBtn.addEventListener("click", function () {
        modal.style.display = "none";
    });

    cancelModalBtn.addEventListener("click", function () {
        modal.style.display = "none";
    });

    window.addEventListener("click", function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("myForm");
    const adicionarBtn = document.getElementById("addBtn");
    const responseMessage = document.getElementById("responseMessage");

    adicionarBtn.addEventListener("click", function () {
        const formData = new FormData(form);
        const formDataObject = {};
        formData.forEach((value, key) => {
            formDataObject[key] = value;
        });

        console.log(formDataObject);

        const apiUrl = `https://localhost:7215/products/add/${username}`;

        fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(formDataObject),
        })

            .then((response) => {

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }

                return response.json();
            })
            .then((data) => {

                alert("New product saved with sucess!");
                window.location.href = `https://localhost:7097/Products`
                closeAddModal();
            })
            .catch((error) => {
                console.error("Erro:", error);
                alert("An error ocurred while saving the product, check out the logs");
            });
    });
});
