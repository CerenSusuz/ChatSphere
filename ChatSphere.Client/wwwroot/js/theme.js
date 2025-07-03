window.toggleTheme = () => {
    const current = document.documentElement.getAttribute("data-theme");
    const newTheme = current === "dark" ? "light" : "dark";
    document.documentElement.setAttribute("data-theme", newTheme);
    sessionStorage.setItem("theme", newTheme);
};

window.initTheme = () => {
    const saved = sessionStorage.getItem("theme") || "light";
    document.documentElement.setAttribute("data-theme", saved);
};

window.getCurrentTheme = () => {
    return document.documentElement.getAttribute("data-theme") || "light";
};
