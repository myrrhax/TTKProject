export default async function protectedRequestFabric(url, method, body) {
    const token = localStorage.getItem('token');
    const refreshUrl = "http://localhost:5001/api/auth/refresh"

    if (token === null) {
        return null;
    }

    let response = await fetch(url, {
        method: method,
        body: body,
        headers: {
            "Authorization": "Bearer " + token
        }
    })

    if (response.status === 401) {
        const refreshResponse = await fetch(refreshUrl);

        if (!refreshResponse.ok) {
            // Refresh не сработал — возвращаем null
            return null;
        }
        localStorage.setItem('token', refreshResponse.token)
        
        // Получили новый токен, повторяем оригинальный запрос
        response = await fetch(url, {
            method: method,
            body: body,
            headers: {
                "Authorization": "Bearer " + token
            }
        })
    }

    return response.ok ? response : null;
}