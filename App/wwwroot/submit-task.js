document.getElementById('taskForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const formData = new FormData(this);
    const jsonData = {};
    formData.forEach((value, key) => { jsonData[key] = value; });

    const res = await fetch('/api/observations/CreateObservationTask', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(jsonData)
    });

    const resultText = document.getElementById('result');
    if (res.ok) {
        resultText.innerText = '任务提交成功！';
        resultText.style.color = 'green';
        this.reset();
    } else {
        resultText.innerText = '任务提交失败。';
        resultText.style.color = 'red';
    }
});