var username = document.getElementById("user").dataset.username;

const checkboxList = [];

const tabela = document.getElementById("dataTable");
const tbody = tabela.getElementsByTagName("tbody")[0];

tabela.style.display = "none";

function showAlert(message, type = 'info') {
    const alertPlaceholder = document.getElementById('liveAlertPlaceholder');
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.role = 'alert';
    alert.innerHTML = `
      ${message}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    alertPlaceholder.appendChild(alert);

    setTimeout(() => {
        alert.classList.remove('show');
        alert.classList.add('fade');
        setTimeout(() => alert.remove(), 150);
    }, 5000);
}


function closeAddModal() {
    const modal = document.getElementById("myModal");
    modal.style.display = "none";
}
function closeRemoveModal() {
    const modal = document.getElementById("remove-modal");
    modal.style.display = "none";
}
function closeEditModal() {
    const modal = document.getElementById("edit-modal");
    modal.style.display = "none";
}

function getToken() {
    const name = 'AuthToken=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const cookies = decodedCookie.split(';');
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i];
        while (cookie.charAt(0) === ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(name) === 0) {
            const token = cookie.substring(name.length, cookie.length);
            return token;
        }
    }
    console.log('Token Not Found');
    return '';
}

document.addEventListener("DOMContentLoaded", function () {
    const token = getToken();
});

document.addEventListener("DOMContentLoaded", function () {
    token = getToken();
    console.log("aqui esta o token resgatado", token)
    loadProducts(username);
    closeAddModal()
});

function loadProducts(username) {
    fetch(`https://localhost:7097/products/GetProductsData`, {
        method: 'GET'
    })

        .then((response) => {
            if (!response.ok) {
                if (response.status === 401) {
                    showAlert("Unauthorized: Please log in to view your products.", "warning");
                    throw new Error("Unauthorized");
                } else {
                    showAlert("An error occurred", "warning");
                    throw new Error("An error occurred");
                }

            }
            return response.json();

        })
        .then((data) => {

            console.log("Json data:", data);

            tbody.innerHTML = "";

            if (data.length === 0) {
                noProductsMessage.style.display = "block";
                return;
            }

            tabela.style.display = "block";

            data.forEach((item) => {

                const newRow = tbody.insertRow();

                const cell1 = newRow.insertCell(0);
                const cell2 = newRow.insertCell(1);
                const cell3 = newRow.insertCell(2);
                const cell4 = newRow.insertCell(3);
                const cell5 = newRow.insertCell(4);
                const cell6 = newRow.insertCell(5);
                const cell7 = newRow.insertCell(6);
                const cell8 = newRow.insertCell(7);

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
                cell3.textContent = item.situation;
                cell4.textContent = item.quantity;
                cell5.textContent = item.currencyCode;
                cell6.textContent = "$" + item.price;
                cell7.textContent = username;
                cell8.textContent = FormatedDate;

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
            if (error.message.includes("401")) {
                showAlert("Unauthorized: Please log in to view your products.", "warning");
            } else {
                showAlert("Failed to load products. Please try again later.", "warning");
            }
        });
}

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
                            'Authorization': `Bearer ${token}`,
                            "Content-Type": "application/json",
                        },
                    })
                        .then((response) => response.json())
                        .then((data) => {
                            closeRemoveModal();
                            showAlert("Product deleted with sucess!", "success");
                            loadProducts(username);
                        })
                        .catch((error) => {
                            if (error.message.includes("401")) {
                                showAlert("Unauthorized: Please log in to delete products.", "warning");
                            } else {
                                showAlert("An error occurred while deleting the product. Please check the logs.", "warning");
                            }
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
                const situation = cells[2].textContent;
                const quantity = cells[3].textContent;
                const currencyCode = cells[4].textContent;
                const price = cells[5].textContent.replace("$", "");
                const date = cells[7].textContent;

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
       <select class="select form-control" id="editSituation" name="situation">
                            <option value="Sold" ${situation === "Sold" ? "selected" : ""}>Sold</option>
                            <option value="Purchased" ${situation === "Purchased" ? "selected" : ""}>Purchased</option>
                        </select>
    </div>
    <div class="mb-3">
      <b><label for="editQuantity" class="form-label">Quantity:</label></b>
      <input type="text" class="form-control" id="editQuantity" value="${quantity}">
    </div>
    <div class="mb-3">
  <b><label for="editCorrencyCode" class="form-label">Currency Code:</label></b>
  <select class="select form-control" id="editCurrencyCode" name="currencyCode">
                            <option value="BRL" ${currencyCode === "BRL" ? "selected" : ""}>Brazillian Real</option>
                            <option value="USD" ${currencyCode === "USD" ? "selected" : ""}>Dollar</option>
                        </select>
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
                    const editedProductSituation = document.getElementById("editSituation").value;
                    const editedQuantity = parseFloat(
                        document.getElementById("editQuantity").value
                    );
                    const editedCurrencyCode = document.getElementById("editCurrencyCode").value;
                    const editedPrice = document.getElementById("editPrice").value;

                    const editedData = {
                        price: editedPrice,
                        situation: editedProductSituation,
                        quantity: editedQuantity,
                        currencyCode: editedCurrencyCode,
                        date: new Date(editedDate),
                        product: editedProduct,

                    };

                    const apiUrl = `https://localhost:7215/products/${id}`;


                    fetch(apiUrl, {
                        method: "PUT",
                        headers: {
                            'Authorization': `Bearer ${token}`,
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
                            loadProducts(username);
                            showAlert("Product updated with sucess!", "success");
                        })
                        .catch((error) => {
                            console.error("Erro:", error);
                            showAlert("An erro ocurred while updating a product, check out the logs", "warning");
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
    const addBtn = document.getElementById("addBtn");

    addBtn.addEventListener("click", function () {
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
                'Authorization': `Bearer ${token}`,
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
                loadProducts(username);
                closeAddModal();
                showAlert("New product saved with sucess!", "success");
            })
            .catch((error) => {
                if (error.message.includes("401")) {
                    showAlert("Unauthorized: Please log in to view your products.", "warning");
                } else {
                    showAlert("An error ocurred while saving the product, check out the logs", "warning");
                }
            });

    });
});
