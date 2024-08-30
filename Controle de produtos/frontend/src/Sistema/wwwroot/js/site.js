var usernamesession = document.getElementById("user").dataset.username;
console.log("Usuario da sessão");
console.log(usernamesession);

localStorage.setItem('username', usernamesession);

console.log(localStorage);

var username = localStorage.getItem('username');
console.log("Usuario resgatado do storage");
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

fetch(`https://localhost:7215/produtos/buscar/${username}`)
    .then((response) => {
        if (!response.ok) {
            throw new Error("Ocorreu um erro");
        }
        console.log(response);
        return response.json();

    })
    .then((data) => {
        console.log("Dados JSON recebidos:", data);
        const tabela = document.getElementById("tabelaDados");

        const tbody = tabela.getElementsByTagName("tbody")[0];

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

            const data = new Date(item.data);

            const opcoesDeFormato = {
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit",
            };

            const formatoData = new Intl.DateTimeFormat("pt-BR", opcoesDeFormato);
            const dataFormatada = formatoData.format(data);

            cell1.textContent = "#" + item.id;
            cell2.textContent = item.produto;
            cell3.textContent = dataFormatada;
            cell4.textContent = item.situacao;
            cell5.textContent = item.quantidade;
            cell6.textContent = item.preco;
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
        console.error("Erro:", error);
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
                    const apiUrl = `https://localhost:7215/produtos/${id}`;

                    fetch(apiUrl, {
                        method: "DELETE",
                        headers: {
                            "Content-Type": "application/json",
                        },
                    })
                        .then((response) => response.json())
                        .then((data) => {
                            alert("Deletado com sucesso!");
                            window.location.href = `https://localhost:7097/Home/Produtos/${username}`
                            closeEditModal();
                        })
                        .catch((error) => {
                            console.error("Ocorreu um erro:", error);
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
                const produto = cells[1].textContent;
                const data = cells[2].textContent;
                const situacao = cells[3].textContent;
                const quantidade = cells[4].textContent;
                const preco = cells[5].textContent;

                const modalEditContent = `

            <form id="editForm">
    <div class="mb-3">
      <b><label for="editProduto" class="form-label">Produto:</label></b>
      <input type="text" class="form-control" id="editProduto" value="${produto}">
    </div>
    <div class="mb-3">
      <b><label for="editData" class="form-label">Data:</label></b>
      <input type="text" class="form-control" id="editData" value="${data}">
    </div>
    <div class="mb-3">
      <b><label for="editSituacao" class="form-label">Situacao:</label></b>
      <input type="text" class="form-control" id="editSituacao" value="${situacao}">
    </div>
    <div class="mb-3">
      <b><label for="editQuantidade" class="form-label">Quantidade:</label></b>
      <input type="text" class="form-control" id="editQuantidade" value="${quantidade}">
    </div>
    <div class="mb-3">
      <b><label for="editPreco" class="form-label">Preço:</label></b>
      <input type="text" class="form-control" id="editPreco" value="${preco}">
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
                    const produtoEditado = document.getElementById("editProduto").value;
                    const dataEditada = document.getElementById("editData").value;
                    const situacaoEditada =
                        document.getElementById("editSituacao").value;
                    const quantidadeEditada = parseFloat(
                        document.getElementById("editQuantidade").value
                    );
                    const precoEditado = document.getElementById("editPreco").value;

                    const editedData = {
                        preco: precoEditado,
                        situacao: situacaoEditada,
                        quantidade: quantidadeEditada,
                        data: new Date(dataEditada),
                        produto: produtoEditado,
                    };

                    const apiUrl = `https://localhost:7215/produtos/${id}`;

                    fetch(apiUrl, {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(editedData),
                    })
                        .then((response) => {

                            if (!response.ok) {
                                console.log("Aqui")
                                console.log(response)
                                throw new Error(`erro HTTP! Status: ${response.status}`);
                            }

                            return response.json();
                        })

                        .then((data) => {
                            console.log("Resposta");
                            console.log(data);
                            alert("Salvo com sucesso!");
                            window.location.href = window.location.href = `https://localhost:7097/Home/Produtos/${usernamesession}`
                        })
                        .catch((error) => {
                            console.error("Erro:", error);
                            alert("Ecorreu um erro ao editar o produto, confira os logs");
                        });

                    modalEdit.style.display = "none";
                });
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", function () {
    setTimeout(function () {
        const rows = document.querySelectorAll("#tabelaDados tbody tr");
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

        const apiUrl = `https://localhost:7215/produtos/adicionar/${username}`;

        fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(formDataObject),
        })

            .then((response) => {

                if (!response.ok) {
                    throw new Error(`erro HTTP! Status: ${response.status}`);
                }

                return response.json();
            })
            .then((data) => {

                alert("Novo produto salvo com sucesso!");
                window.location.href = `https://localhost:7097/Home/Produtos/${username}`
                closeAddModal();
            })
            .catch((error) => {
                console.error("Erro:", error);
                alert("Ocorreu um erro ao salvar produto, confira os logs");
            });
    });
});
