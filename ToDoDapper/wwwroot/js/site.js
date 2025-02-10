document.getElementById("addTaskBtn").addEventListener("click", function () {
    let container = document.getElementById("newTaskContainer");
    let addTaskBtn = document.getElementById("addTaskBtn");

    if (document.getElementById("newTaskInput")) return;

    let input = document.createElement("input"); // create input
    input.type = "text";
    input.className = "form-control mb-2";
    input.placeholder = "Введите задачу";
    input.id = "newTaskInput";

    let saveButton = document.createElement("button"); // create buttton for save
    saveButton.textContent = "Сохранить";
    saveButton.id = "saveButton";
    saveButton.className = "btn btn-outline-success btn-sm ms-2";
    saveButton.innerHTML = `<img src="/plus.svg"/>`;

    let cancelButton = document.createElement("button"); // Кнопка отмены
    cancelButton.textContent = "Отмена";
    cancelButton.className = "btn btn-outline-danger btn-sm ms-2";
    cancelButton.innerHTML = `<img src="/cancel.svg"/>`;
    cancelButton.onclick = function () {
        input.remove();
        saveButton.remove();
        cancelButton.remove();
        addTaskBtn.style.display = "block"; // Вернуть кнопку "Добавить задачу"
    };

    saveButton.addEventListener("click", function () {
        handleSaveClick(input, saveButton, cancelButton);
    });

    container.appendChild(input);
    container.appendChild(saveButton);
    container.appendChild(cancelButton);
    addTaskBtn.style.display = "none"; // Скрыть кнопку "Добавить задачу"

    input.focus();
});

async function handleSaveClick(input, saveButton, cancelButton) { //добавление задачи
    let taskTitle = input.value.trim();
    let addTaskBtn = document.getElementById("addTaskBtn"); 

    if (taskTitle !== "") {
        var response = fetch("/Index?handler=Add&title=" + taskTitle)
        .then(Response => {
            if (Response.ok) {
               addTaskToList(taskTitle);
                    input.remove();
                    saveButton.remove();
                    cancelButton.remove();
                    addTaskBtn.style.display = "block"; 
            }
        });

    }
}
function addTaskToList(title, taskId) {
    let list = document.getElementById("TaskList"); 

    let newItem = document.createElement("li");
    newItem.className = "list-group-item d-flex justify-content-between align-items-start";
    newItem.innerHTML = `
        <div class="ms-2 me-auto">
            <label>${title}</label>
            <button type="button" class="btn btn-outline-success btn-sm">
                <img src="/done.svg"/>
            </button>
            //<button type="button" class="btn btn-outline-danger btn-sm" onclick="deleteTask(${taskId}, this)">
            //    <img src="/delete.svg"/>
            //</button>
        </div>`;
    list.appendChild(newItem);
}
