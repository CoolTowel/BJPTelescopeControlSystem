document.addEventListener("DOMContentLoaded", async () => {
    const containers = document.querySelectorAll(".auto-table");

    for (const container of containers) {
        const api = container.dataset.api;
        const title = container.dataset.title || "数据表格";
        if (!api) continue;

        const response = await fetch(api);
        const data = await response.json();
        if (!Array.isArray(data) || data.length === 0) {
            container.innerHTML = "<p>无数据</p>";
            continue;
        }

        const keys = Object.keys(data[0]);
        const box = document.createElement("div");
        box.className = "table-box";

        const header = document.createElement("div");
        header.className = "table-header";
        header.innerHTML = `<span>${title}</span><span class="arrow">▶</span>`;
        box.appendChild(header);

        const table = document.createElement("table");
        table.className = "auto-generated";

        const thead = document.createElement("thead");
        const headerRow = document.createElement("tr");
        keys.forEach(key => {
            const th = document.createElement("th");
            th.textContent = key;
            th.classList.add("sortable");
            th.dataset.key = key;
            headerRow.appendChild(th);
        });
        thead.appendChild(headerRow);
        table.appendChild(thead);

        const tbody = document.createElement("tbody");
        data.forEach(row => {
            const tr = document.createElement("tr");
            keys.forEach(key => {
                const td = document.createElement("td");
                td.textContent = row[key];
                tr.appendChild(td);
            });
            tbody.appendChild(tr);
        });

        table.appendChild(tbody);

        const content = document.createElement("div");
        content.appendChild(table);
        box.appendChild(content);

        let collapsed = false;
        header.addEventListener("click", () => {
            collapsed = !collapsed;
            content.style.display = collapsed ? "none" : "block";
            header.classList.toggle("collapsed", collapsed);
        });

        container.appendChild(box);

        // 排序逻辑
        let sortState = {};
        headerRow.querySelectorAll("th.sortable").forEach(th => {
            th.addEventListener("click", () => {
                const key = th.dataset.key;
                const current = sortState[key] || "none";
                const next = current === "asc" ? "desc" : "asc";
                sortState = { [key]: next };

                // 清除其他列排序状态
                headerRow.querySelectorAll("th").forEach(t => {
                    t.classList.remove("asc", "desc");
                });
                th.classList.add(next);

                const rows = Array.from(tbody.querySelectorAll("tr"));
                rows.sort((a, b) => {
                    const av = a.children[keys.indexOf(key)].textContent;
                    const bv = b.children[keys.indexOf(key)].textContent;
                    return next === "asc" ? av.localeCompare(bv) : bv.localeCompare(av);
                });

                rows.forEach(r => tbody.appendChild(r));
            });
        });
    }
});
