export const formatDate = (date: Date, format:  "short" | "long" | "full" = "short") => {
    switch(format){
        case "short":
            return date.toLocaleDateString("ru-RU");
        case "long":
            return date.toLocaleDateString("ru-RU", {
                year: "numeric",
                month: "long",
                day: "numeric",
            });
        case "full":
            return date.toLocaleString("ru-RU");
    }
}