import React from "react"

export interface BaseInputProps extends React.InputHTMLAttributes<HTMLInputElement>{
    error?: boolean,
}

export interface CustomInputProps extends BaseInputProps{
    onSend: (query: string) => void
}

export interface SearchInputProps extends BaseInputProps{
    onSearch: (query: string) => void,
}

